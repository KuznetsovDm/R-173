using System;
using System.Linq;
using System.Net;
using CSharpFunctionalExtensions;
using P2PMulticastNetwork.Network;
using System.Net.Sockets;
using R_173.Interfaces;
using System.Threading.Tasks;
using System.Threading;
using P2PMulticastNetwork.Extensions;
using R_173.BE;
using P2PMulticastNetwork.Rpc;
using AustinHarris.JsonRpc;
using System.Collections.Concurrent;
using System.IO;
using Newtonsoft.Json;

namespace R_173.BL
{
    public enum NetworkTaskState
    {
        //не в задаче.
        NotInTask,
        //ожидание подключения по tcp.
        Waiting,
        //Подключение по tcp.
        Connecting,
        Resolving,
        Applyed,
        InTask
    }

    public class RemoteService : JsonRpcService
    {
        public event EventHandler Confirm;

        public bool IsConfirm { get; private set; }

        [JsonRpcMethod("Confirm")]
        private void OnConfirm()
        {
            IsConfirm = true;
            Confirm?.Invoke(this, EventArgs.Empty);
        }

        public void ReloadInitialState()
        {
            IsConfirm = false;
        }
    }

    public class TaskService : JsonRpcService, INetworkTaskScheduler
    {
        private readonly IRedistributableLocalConnectionTable _table;
        private NetService _netService;
        private NetMode _mode;
        private TcpRpcHandler _connection;
        private RemoteService _remoteService;
        private bool _confirmed;
        private NetworkTask _currentTask;

        public TaskService(NetService netService)
        {
            _netService = netService;
            _remoteService = new RemoteService();
            _remoteService.Confirm += OnRemoteConfirm;
        }

        private void SendOfferTaskIfActiveAndConfirmedBoth()
        {
            if (_confirmed && _mode == NetMode.Active && _remoteService.IsConfirm)
            {
                var task = new NetworkTask();
                _connection?.SendRequest("OfferTask", task);
            }
        }

        private void OnRemoteConfirm(object sender, EventArgs e)
        {
            try
            {
                SendOfferTaskIfActiveAndConfirmedBoth();
            }
            catch (SocketException)
            {
                _connection.Dispose();
                _connection = null;
                _remoteService.ReloadInitialState();
            }
        }

        public void Confirm()
        {
            if (_connection != null)
            {
                try
                {
                    _connection.SendRequest(nameof(Confirm));
                    SendOfferTaskIfActiveAndConfirmedBoth();
                }
                catch (SocketException)
                {
                    _connection.Dispose();
                    _connection = null;
                    _remoteService.ReloadInitialState();
                }
            }
            // todo: notify that task were applied
            // Starting Task
            // call TaskStarted(task)
        }

        [JsonRpcMethod("OfferTask")]
        private void OnOfferTask(NetworkTask task)
        {
            _currentTask = task;
            TaskStarted?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler TaskCreated;
        public event EventHandler TaskStarted;

        public async void Start()
        {
            var tuple = await _netService.WaitForOneConnection();
            var connection = tuple.Item1;
            _mode = tuple.Item2;

            _connection = connection;

            TaskEx.Run(() =>
            {
                while (true)
                {
                    var response = connection.ReceiveString();
                    Rpc.Handle(response);
                }
            }).ContinueWith(x =>
            {
                _connection = null;
            }, TaskContinuationOptions.OnlyOnFaulted);

            // todo: start scheduler
            // tcp listener separate task
            // if no connections
            // _table.for try to connect
            // stop if tcplistener.connected
            // get task definition -> call TaskCreated
            // wait for Confirm or Cancel, if disconnected, then back to listen and tell TaskDestroyed
        }

        public void Stop()
        {
            try
            {
                if (_connection != null)
                {
                    _connection.Dispose();
                    _connection = null;
                }
            }
            catch (Exception)
            {
            }
        }
    }

    public enum NetMode
    {
        Passive,
        Active
    }

    public class NetService : IDisposable
    {
        TcpListener _listener;
        private readonly IRedistributableLocalConnectionTable _connectionTable;
        private readonly RadioSettings _settings;

        public NetService(int port, IRedistributableLocalConnectionTable connectionTable, RadioSettings settings)
        {
            int maxConnectionQueue = 1;
            _listener = new TcpListener(port);
            _listener.Start();
            _listener.Server.Listen(maxConnectionQueue);
            _connectionTable = connectionTable;
            _settings = settings;
        }

        public async Task<Tuple<TcpRpcHandler, NetMode>> WaitForOneConnection()
        {
            var cts = new CancellationTokenSource();
            var search = SearchOne(cts.Token);
            var listen = ListenOne(cts.Token);
            var result = await TaskEx.WhenAny(search, listen);
            cts.Cancel();
            var mode = search == result ? NetMode.Active : NetMode.Passive;
            var handler = new TcpRpcHandler(result.Result);
            return Tuple.Create(handler, mode);
        }

        public async Task<TcpClient> SearchOne(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                foreach (var connection in _connectionTable.AvaliableDevices.Select(x => x.Endpoint)
                    .Where(x => !x.Address.Equals(_settings.LocalIp)))
                {
                    if (token.IsCancellationRequested) return null;

                    var client = new TcpClient();
                    try
                    {
                        await Task.Factory.FromAsync((callback, obj) =>
                            client.BeginConnect(connection.Address, connection.Port, callback, obj),
                            client.EndConnect, null)
                            .HandleCancellation(token);
                        //await TaskEx.Run(() => client.Connect(connection), token);

                        return client;
                    }
                    catch (Exception)
                    {
                        client.Close();
                    }
                }

                await TaskEx.Delay(TimeSpan.FromSeconds(1), token);
            }

            return null;
        }

        public async Task<TcpClient> ListenOne(CancellationToken token)
        {
            return await Task.Factory.FromAsync(_listener.BeginAcceptTcpClient, _listener.EndAcceptTcpClient, null)
                .HandleCancellation(token);
            //return await TaskEx.Run(() => _listener.AcceptTcpClient(), token);
        }

        public void Dispose()
        {
            _listener?.Server.Close();
            _listener = null;
        }
    }

    public class NetworkTask
    {
        public Guid AcceptorId { get; set; }
        public Guid OwnerId { get; set; }
        public string Description { get; set; }
        public int Frequency { get; set; }
        public NetworkTaskState State { get; set; }
    }

    public class NetworkTaskIpResolver
    {
        private readonly IRedistributableLocalConnectionTable _table;

        public NetworkTaskIpResolver(IRedistributableLocalConnectionTable table)
        {
            _table = table;
        }

        public Result<IPEndPoint> Resolve(Guid id)
        {
            var device = _table.AvaliableDevices.FirstOrDefault(x => x.Id == id);
            return device is null
                ? Result.Fail<IPEndPoint>("Could't find endpoint")
                : Result.Ok(device.Endpoint);
        }
    }

    public class TaskCreatedEventArgs : EventArgs
    {
        /// <summary>
        /// Номер частоты
        /// </summary>
        public int FrequencyNumber { get; set; }

        /// <summary>
        /// Частота
        /// </summary>
        public int Frequency { get; set; }
    }
}
