using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using P2PMulticastNetwork;

namespace R_173
{
    public class RadioManager
    {
        public IDataTransmitter _transmitter;
        public IDataMiner _dataMiner;

        public RadioManager()
        {
            var transmitterOptions = MulticastConnectionOptions.Create(exclusiveAddressUse: false, useBind: false);
            _transmitter = new UdpMulticastConnection(transmitterOptions);

            _dataMiner = new DataEngineMiner();
            var receiveOptions = MulticastConnectionOptions.Create(exclusiveAddressUse: false);
            var receiver = new UdpMulticastConnection(receiveOptions);
        }
    }
}
