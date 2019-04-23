using System;
using System.Threading.Tasks;
using P2PMulticastNetwork;
using P2PMulticastNetwork.Model;
using R_173.Interfaces;

namespace R_173.BL.Handlers
{
	public class NetworkTaskPipelineHandler : IInvoker, INetworkTaskListener
	{
		public async Task Invoke(DataModel model, PipelineDelegate<DataModel> next)
		{
			if (model.NetworkTask != null 
					&& Math.Abs(model.NetworkTask.Frequency - model.RadioModel.Frequency) < 0.001)
			{
				OnDataAvailable?.Invoke(this, new DataEventArgs<NetworkTaskData>(model.NetworkTask));
			}

			await next.Invoke(model);
		}

		public event EventHandler<DataEventArgs<NetworkTaskData>> OnDataAvailable;
	}
}
