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
using R_173.Helpers;
using R_173.BE;

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

    public class TaskService : INetworkTaskScheduler
    {
        private readonly IRedistributableLocalConnectionTable _table;
        private NetService _netService;

        public TaskService(IRedistributableLocalConnectionTable table, NetService netService)
        {
            _table = table;
            _table.OnConnected += OnConnected;
            _table.OnDisconnected += OnDisconnected;
            _netService = netService;
        }

        private void OnDisconnected(object sender, ConnectionArgs e)
        {
        }

        private void OnConnected(object sender, ConnectionArgs e)
        {
        }

        public void OnAppyTask(NetworkTask task)
        {
            //todo: add to table.
        }

        public void DeclineTask(NetworkTask task)
        {
            //todo: remove from table.
        }

        public event EventHandler TaskCreated;
        public event EventHandler TaskStarted;

        public async void Start()
        {
            var connection = await _netService.WaitForOneConnection();
            // todo: start scheduler
            // tcp listener separate task
            // if no connections
            // _table.for try to connect
            // stop if tcplistener.connected
            // get task definition -> call TaskCreated
            // wait for Confirm or Cancel, if disconnected, then back to listen and tell TaskDestroyed
        }

        public void Confirm()
        {
            // todo: notify that task were applied
            // Starting Task
            // call TaskStarted(task)

            throw new NotImplementedException();
        }

        public void Stop()
        {
            // todo: stop scheduler
            throw new NotImplementedException();
        }
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

        public async Task<TcpClient> WaitForOneConnection()
        {
            var cts = new CancellationTokenSource();
            var search = SearchOne(cts.Token);
            var listen = ListenOne(cts.Token);
            var result = await TaskEx.WhenAny(search, listen);
            cts.Cancel();
            return result.Result;
        }

        public async Task<TcpClient> SearchOne(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                foreach (var connection in _connectionTable.AvaliableDevices.Select(x => x.Endpoint)
                    .Where(x => !x.Address.Equals(_settings.LocalIp)))
                {
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

                await TaskEx.Delay(TimeSpan.FromSeconds(1));
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
            if (_listener != null)
                _listener.Server.Close();
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
