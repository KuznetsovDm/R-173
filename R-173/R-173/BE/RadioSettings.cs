using R_173.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace R_173.BE
{
    public class RadioSettings
    {
        public IPAddress LocalIp { get; set; }
        public Guid NetworkToken { get; set; }

        public RadioSettings()
        {
            LocalIp = IpHelper.GetLocalIpAddress();
            NetworkToken = Guid.NewGuid();
        }
    }
}
