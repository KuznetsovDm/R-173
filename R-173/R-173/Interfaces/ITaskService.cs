using System;
using System.Threading;
using System.Threading.Tasks;
using P2PMulticastNetwork.Model;
using R_173.BL.Tasks;
using Task = System.Threading.Tasks.Task;

namespace R_173.Interfaces
{
	public class CreatedNetworkTaskData
	{
		public Guid Id { get; set; }
		public int ComputerNumber { get; set; }
		public int FrequencyNumber { get; set; }
		public int Frequency { get; set; }
	}

	public interface ITaskService
	{
		event EventHandler<DataEventArgs<CreatedNetworkTaskData>> TaskCreated;
		event EventHandler<DataEventArgs<CreatedNetworkTaskData>> TaskStarted;

		void Start();
		void Confirm();
		void Stop();
	}

	public class DummyTaskService : ITaskService
	{
		public event EventHandler<DataEventArgs<CreatedNetworkTaskData>> TaskCreated;
		public event EventHandler<DataEventArgs<CreatedNetworkTaskData>> TaskStarted;

		private CreatedNetworkTaskData _taskData;
		private CancellationTokenSource _cancellationTokenSource;

		public void Start()
		{
			_cancellationTokenSource = new CancellationTokenSource();

			Task.Factory.StartNew(() =>
			{
				try
				{
					TaskEx.Delay(5000, _cancellationTokenSource.Token).GetAwaiter().GetResult();
					if (_cancellationTokenSource.IsCancellationRequested) return;
				}
				catch (TaskCanceledException)
				{
					return;
				}

				_taskData = new CreatedNetworkTaskData
				{
					Id = Guid.NewGuid(),
					ComputerNumber = TaskHelper.GenerateComputerNumber(),
					Frequency = TaskHelper.GenerateValidR173Frequency(),
					FrequencyNumber = TaskHelper.GenerateValidR173NumpadValue()
				};

				TaskCreated?.Invoke(this, new DataEventArgs<CreatedNetworkTaskData>(_taskData));
			});
		}

		public void Confirm()
		{
			Task.Factory.StartNew(() =>
			{
				try
				{
					TaskEx.Delay(5000, _cancellationTokenSource.Token).GetAwaiter().GetResult();
					if (_cancellationTokenSource.IsCancellationRequested) return;
				}
				catch (TaskCanceledException)
				{
					return;
				}

				TaskStarted?.Invoke(this, new DataEventArgs<CreatedNetworkTaskData>(_taskData));
			});
		}

		public void Stop()
		{
			_cancellationTokenSource.Cancel();
			_taskData = null;
		}
	}
}
