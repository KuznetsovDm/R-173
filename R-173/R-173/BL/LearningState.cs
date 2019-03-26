using CSharpFunctionalExtensions;
using P2PMulticastNetwork.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace R_173.Models
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

    public class TaskTable
    {
        private readonly IRedistributableLocalConnectionTable _table;

        public TaskTable(IRedistributableLocalConnectionTable table)
        {
            _table = table;
            _table.OnConnected += OnConnected;
            _table.OnDisconnected += OnDisconnected;
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
            if (device is null)
                return Result.Fail<IPEndPoint>("Could't find endpoint");
            return Result.Ok(device.Endpoint);
        }
    }
}
