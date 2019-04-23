using P2PMulticastNetwork.Model;
using R_173.BE;
using R_173.Interfaces;

namespace R_173.BL.Tasks
{
	public class NetworkTask : ITask
	{
		private readonly INetworkTaskManager _networkTaskManager;
		private readonly NetworkTaskData _taskData;
		private readonly INetworkTaskListener _networkTaskListener;
		private int _successfullDataReceivedCount;
		private const int MinimumSuccessfullDataReceivedCountToSuccessTask = 5;

		public NetworkTask(INetworkTaskManager networkTaskManager,
			NetworkTaskData taskData,
			INetworkTaskListener networkTaskListener)
		{
			_networkTaskManager = networkTaskManager;
			_taskData = taskData;
			_networkTaskListener = networkTaskListener;
			_networkTaskListener.OnDataAvailable += OnDataAvailable;
		}

		private void OnDataAvailable(object sender, DataEventArgs<NetworkTaskData> args)
		{
			if (_taskData.Id == args.Data.Id) _successfullDataReceivedCount++;
		}

		public void Start()
		{
			_networkTaskManager.CurrentNetworkTask = _taskData;
		}

		public Message Stop()
		{
			_networkTaskManager.CurrentNetworkTask = null;
			_networkTaskListener.OnDataAvailable -= OnDataAvailable;

			Message message = null;
			if (_successfullDataReceivedCount < MinimumSuccessfullDataReceivedCountToSuccessTask)
			{
				message = new Message
				{
					Header = $"Получено недостаточное количество пакетов: {_successfullDataReceivedCount}"
				};
			}

			return message;
		}
	}
}
