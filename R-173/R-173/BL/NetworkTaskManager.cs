using P2PMulticastNetwork.Model;
using R_173.Interfaces;

namespace R_173.BL
{
	public class NetworkTaskManager : INetworkTaskManager
	{
		public NetworkTaskData CurrentNetworkTask { get; set; }
	}
}
