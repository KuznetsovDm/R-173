using System;
using P2PMulticastNetwork.Model;

namespace R_173.Interfaces
{
	public interface INetworkTaskListener
	{
		event EventHandler<DataEventArgs<NetworkTaskData>> OnDataAvailable;
	}
}
