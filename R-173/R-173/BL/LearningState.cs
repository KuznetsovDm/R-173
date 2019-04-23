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
using P2PMulticastNetwork.Model;
using R_173.BE;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace R_173.BL
{
    public enum NetworkServiceState
    {
        Started,
        Finding,
        RequestForConfirm,
        Confirmed,
        Stoped,
        Closed,
    }

    public class ConfirmationResult
    {
        public bool Result { get; set; }
    }

    public class NetworkTaskService : ITaskService
    {
        private readonly NetService _netService;

        public NetworkTaskService(NetService netService)
        {
            _netService = netService;
            State = NetworkServiceState.Stoped;
        }

        public NetworkServiceState State { get; private set; }

        public event EventHandler<DataEventArgs<CreatedNetworkTaskData>> TaskCreated;
        public event EventHandler<DataEventArgs<CreatedNetworkTaskData>> TaskStarted;
        private CancellationTokenSource _cts;
        private TaskCompletionSource<ConfirmationResult> _completationConfirm;

        public async void Start()
        {
            if (State == NetworkServiceState.Stoped && _cts == null)
            {
                _completationConfirm = new TaskCompletionSource<ConfirmationResult>();
                _cts = new CancellationTokenSource();
                State = NetworkServiceState.Started;
                await TaskLoop(_cts.Token);
            }
            else
                throw new InvalidOperationException();
            // todo: start scheduler
            // tcp listener separate task
            // if no connections
            // _table.for try to connect
            // stop if tcplistener.connected
            // get task definition -> call TaskCreated
            // wait for Confirm or Cancel, if disconnected, then back to listen and tell TaskDestroyed
        }

        private async Task TaskLoop(CancellationToken ct)
        {
            try
            {
                while (!ct.IsCancellationRequested)
                {
                    State = NetworkServiceState.Finding;
                    var result = await _netService.WaitForOneConnection(ct);
                    var connection = result.Key;
                    var commingType = result.Value;
                    var writer = new BinaryWriter(connection.GetStream());
                    var reader = new BinaryReader(connection.GetStream());

                    if (commingType == ConnectionCommingType.FromListen)
                    {
                        var jsontask = reader.ReadString();
                        var task = JsonConvert.DeserializeObject<CreatedNetworkTaskData>(jsontask);
                        TaskCreated?.Invoke(this, new DataEventArgs<CreatedNetworkTaskData>(task));
                    }
                    else
                    {
                        //todo: generate task...
                        var task = new CreatedNetworkTaskData()
                        {
                            Frequency = 33100,
                            FrequencyNumber = new Random().Next(0, 10),
                            Id = Guid.NewGuid()
                        };
                        var jsontask = JsonConvert.SerializeObject(task);
                        writer.Write(jsontask);
                        writer.Flush();
                        TaskCreated?.Invoke(this, new DataEventArgs<CreatedNetworkTaskData>(task));
                    }
                    State = NetworkServiceState.RequestForConfirm;

                    var remoteConfirm = TaskEx.Run(() =>
                    {
                        var confirmationResult = reader.ReadString();
                        var isConfirmed = JsonConvert.DeserializeObject<ConfirmationResult>(confirmationResult);
                        return isConfirmed;
                    });

                    var localConfirm = _completationConfirm.Task.ContinueWith(t =>
                    {
                        var cjs = JsonConvert.SerializeObject(t.Result);
                        writer.Write(cjs);
                        writer.Flush();
                        return t.Result;
                    }, TaskContinuationOptions.NotOnFaulted | TaskContinuationOptions.NotOnCanceled);

                    var bothConfirmation = await TaskEx.WhenAll(remoteConfirm, localConfirm);
                    bool isBothConfimed = bothConfirmation.All(x => x.Result);
                    if (isBothConfimed)
                    {
                        TaskStarted?.Invoke(this, new DataEventArgs<CreatedNetworkTaskData>());
                    }
                }
            }
            catch (TaskCanceledException) { }
            catch (Exception ex)
            {
                SimpleLogger.Log(ex);
            }
        }

        public void Confirm()
        {
            if (State == NetworkServiceState.RequestForConfirm)
            {
                _completationConfirm.SetResult(new ConfirmationResult() { Result = true });
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public void Stop()
        {
            if (_completationConfirm.Task.Status == TaskStatus.Running)
            {
                _completationConfirm?.SetCanceled();
            }
            _completationConfirm = null;
            if (_cts == null)
                throw new InvalidOperationException();

            if (State != NetworkServiceState.Stoped)
            {
                State = NetworkServiceState.Stoped;
                _cts.Cancel();
                _cts = null;
            }
        }
    }

    public class NetService : IDisposable
    {
        private TcpListener _listener;
        private readonly IRedistributableLocalConnectionTable _connectionTable;
        private readonly RadioSettings _settings;

        public NetService(int port, IRedistributableLocalConnectionTable connectionTable, RadioSettings settings)
        {
            _listener = new TcpListener(port);
            _connectionTable = connectionTable;
            _settings = settings;
        }

        public async Task<KeyValuePair<TcpClient, ConnectionCommingType>> WaitForOneConnection(CancellationToken token)
        {
            var search = SearchOne(token);
            var listen = ListenOne(token);
            var result = await TaskEx.WhenAny(search, listen);
            var commingType = search == result ? ConnectionCommingType.FromConnect : ConnectionCommingType.FromListen;
            return new KeyValuePair<TcpClient, ConnectionCommingType>(result.Result, commingType);
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

        public Task<TcpClient> ListenOne(CancellationToken token)
        {
            return TaskEx.Run<TcpClient>(async () =>
            {
                _listener.Start();
                _listener.Server.Listen(1);
                var result = await Task.Factory.FromAsync(_listener.BeginAcceptTcpClient, _listener.EndAcceptTcpClient, null)
                .HandleCancellation(token);
                _listener.Stop();
                return result;
            });
            //return await TaskEx.Run(() => _listener.AcceptTcpClient(), token);
        }

        public void Dispose()
        {
            _listener?.Server.Close();
            _listener = null;
        }
    }

    public enum ConnectionCommingType
    {
        Undefined,
        FromListen,
        FromConnect
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
