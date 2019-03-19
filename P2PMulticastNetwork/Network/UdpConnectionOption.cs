using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace P2PMulticastNetwork
{
    public class UdpConnectionOption
    {
        public IPAddress Address { get; set; } = IPAddress.Any;
        public int Port { get; set; } = 33100;
    }
}
