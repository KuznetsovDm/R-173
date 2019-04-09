using System.Net;

namespace P2PMulticastNetwork.Network
{
    public class UdpConnectionOption
    {
        public IPAddress Address { get; set; } = IPAddress.Any;
        public int Port { get; set; } = 33100;
    }
}
