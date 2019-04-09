using R_173.Helpers;
using System;
using System.Net;

namespace R_173.BE
{
    public class RadioSettings
    {
        public IPAddress LocalIp { get; set; }
        public Guid NetworkToken { get; set; }

        public RadioSettings()
        {
	        try
	        {
		        LocalIp = IpHelper.GetLocalIpAddress();
			}
			catch (Exception)
	        {
		        LocalIp = IPAddress.Loopback;
	        }

            NetworkToken = Guid.NewGuid();
        }
    }
}
