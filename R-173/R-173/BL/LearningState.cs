using System;
using System.Linq;
using System.Net;
using CSharpFunctionalExtensions;
using P2PMulticastNetwork.Network;
using R_173.Interfaces;

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

    public class TaskTable : INetworkTaskScheduler
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

	    public event EventHandler TaskCreated;
	    public event EventHandler TaskStarted;

	    public void Start()
	    {
			// todo: start scheduler
			// tcp listener separate task
			// if no connections
			// _table.for try to connect
			// stop if tcplistener.connected
			// get task definition -> call TaskCreated
			// wait for Confirm or Cancel, if disconnected, then back to listen and tell TaskDestroyed
		
		    throw new NotImplementedException();
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
