using System.Net;

namespace P2PMulticastNetwork.Network
{
    public class MulticastConnectionOptions
    {
        public int Port { get; set; }

        public IPAddress MulticastAddress { get; set; }

        public bool ExclusiveAddressUse { get; set; }

        public bool UseBind { get; set; }

        public bool MulticastLoopback { get; set; }

        private MulticastConnectionOptions()
        {

        }

        public static MulticastConnectionOptions Create(int port = 10000, bool exclusiveAddressUse = true, bool multicastLoopback=true, 
            bool useBind=true, string ipAddress = "239.0.0.0")
        {
            return new MulticastConnectionOptions
            {
                Port = port,
                ExclusiveAddressUse = exclusiveAddressUse,
                MulticastAddress = IPAddress.Parse(ipAddress),
                UseBind = useBind,
                MulticastLoopback = multicastLoopback
            };
        }
    }
}