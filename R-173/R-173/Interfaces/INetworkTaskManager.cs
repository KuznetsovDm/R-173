using P2PMulticastNetwork.Model;

namespace R_173.Interfaces
{
	public interface INetworkTaskManager
	{
		NetworkTaskData CurrentNetworkTask { get; set; }
	}
}
