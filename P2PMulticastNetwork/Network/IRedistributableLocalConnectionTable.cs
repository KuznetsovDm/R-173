using P2PMulticastNetwork;
using P2PMulticastNetwork.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace P2PMulticastNetwork
{
    public interface IRedistributableLocalConnectionTable : IDisposable
    {
        IEnumerable<NotificationData> AvaliableDevices { get; }

        void Register(NotificationData notification);

        event EventHandler<ConnectionArgs> OnConnected;

        event EventHandler<ConnectionArgs> OnDisconnected;
    }

    [Serializable]
    public class NotificationData
    {
        public Guid Id { get; set; }
        public IPEndPoint Endpoint { get; set; }
    }

    public class RedistLocalConnectionTable : IRedistributableLocalConnectionTable
    {
        private UdpClient _listener = new UdpClient();
        private AsyncLoopTimer _timer;
        private ConcurrentDictionary<Guid, InternalTokenized> _table = new ConcurrentDictionary<Guid, InternalTokenized>();
        private IPEndPoint _connection;
        private RedistLocalConnectionTableRegistrator _registrator;

        public RedistLocalConnectionTable(UdpConnectionOption options, RedistributableTableOption tableOption)
        {
            OnConnected += delegate { };
            OnDisconnected += delegate { };
            InitializeListener(options);
            _registrator = new RedistLocalConnectionTableRegistrator(options.Port, tableOption.NotifyTime);
            _timer = new AsyncLoopTimer(tableOption.LoopTime, () =>
            {
                foreach (var value in _table)
                {
                    if (DateTime.UtcNow.TimeOfDay.Subtract(value.Value.TimeStamp) > tableOption.ExpirationTime)
                    {
                        _table.TryRemove(value.Key, out var info);
                        var args = new ConnectionArgs(info.Data);
                        OnDisconnected(this, args);
                    }
                }
            });
        }

        private void InitializeListener(UdpConnectionOption option)
        {
            _listener.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            _listener.ExclusiveAddressUse = false;
            _connection = new IPEndPoint(option.Address, option.Port);
            _listener.Client.Bind(_connection);
            _listener.BeginReceive(AsyncHandle, _listener);
        }

        private void AsyncHandle(IAsyncResult ar)
        {
            var listener = (UdpClient)ar.AsyncState;
            var data = listener.EndReceive(ar, ref _connection);

            if (data.TryDeserialize<NotificationData>(out var notifyObject))
            {
                HandleConnection(notifyObject);
            }
            _listener.BeginReceive(AsyncHandle, listener);
        }

        private void HandleConnection(NotificationData notifyObject)
        {
            var token = _table.GetOrAdd(notifyObject.Id, (_) =>
            {
                var args = new ConnectionArgs(notifyObject);
                OnConnected(this, args);
                return new InternalTokenized { Data = notifyObject };
            });
            token.TimeStamp = DateTime.UtcNow.TimeOfDay;
        }

        public IEnumerable<NotificationData> AvaliableDevices { get => _table.Values.Select(x => x.Data); }

        public event EventHandler<ConnectionArgs> OnConnected;

        public event EventHandler<ConnectionArgs> OnDisconnected;

        public void Dispose()
        {
            if (_listener != null)
            {
                _listener.Close();
                _listener = null;
            }
            _timer.Dispose();
        }

        public void Register(NotificationData notification)
        {
            _registrator.Register(notification);
        }

        public class RedistributableTableOption
        {
            public TimeSpan ExpirationTime { get; set; } = TimeSpan.FromSeconds(5);
            public TimeSpan LoopTime { get; set; } = TimeSpan.FromSeconds(3);
            public TimeSpan NotifyTime { get; set; } = TimeSpan.FromSeconds(2);
        }

        private class InternalTokenized
        {
            public TimeSpan TimeStamp { get; set; }
            public NotificationData Data { get; set; }
        }

        private class AsyncLoopTimer : IDisposable
        {
            private TimeSpan _delay;
            private Action _todo;
            private CancellationTokenSource _cancelToken;

            public AsyncLoopTimer(TimeSpan delay, Action todo)
            {
                _delay = delay;
                _todo = todo;
                _cancelToken = new CancellationTokenSource();
                Loop();
            }

            private void Loop()
            {
                TaskEx.Run(async () =>
                {
                    while (true)
                    {
                        await TaskEx.Delay(_delay);
                        _todo();
                    }
                },
                _cancelToken.Token);
            }


            public void Dispose()
            {
                if (_cancelToken != null)
                    _cancelToken.Cancel();
                _cancelToken = null;
            }
        }
        public class RedistLocalConnectionTableRegistrator : IDisposable
        {
            private UdpClient _client;
            private AsyncLoopTimer _timer;
            private ConcurrentBag<byte[]> _registedConnections = new ConcurrentBag<byte[]>();

            public RedistLocalConnectionTableRegistrator(int port, TimeSpan sendDelay)
            {
                _client = new UdpClient();
                var ep = new IPEndPoint(IPAddress.Broadcast, port);
                _timer = new AsyncLoopTimer(sendDelay, () =>
                {
                    foreach (var bytes in _registedConnections)
                    {
                        _client.Send(bytes, bytes.Length, ep);
                    }
                });
            }

            public void Register(NotificationData data)
            {
                if (data.TrySerialize(out var result))
                {
                    _registedConnections.Add(result);
                }
            }

            public void Dispose()
            {
                _timer.Dispose();
                _client = null;
            }
        }
    }


    public class ConnectionArgs : EventArgs
    {
        public NotificationData Info { get; }

        public ConnectionArgs(NotificationData data)
        {
            Info = data ?? throw new ArgumentNullException();
        }
    }
}
