using System.Net;
using System.Net.Sockets;

namespace R_173.Helpers
{
    public class IpHelper
    {
        public static IPAddress GetLocalIpAddress()
        {
            const string customIp = "10.0.0.0";
            const int customPort = 64000;
            var ep = new IPEndPoint(IPAddress.Parse(customIp), customPort);

            using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.IP))
            {
                socket.Connect(ep);
                var localEp = socket.LocalEndPoint as IPEndPoint;
                return localEp.Address;
            }
        }
    }
}
