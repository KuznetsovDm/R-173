using System;

namespace P2PMulticastNetwork.Model
{
    public struct ReceivableRadioModel
    {
        public int Frequency { get; set; }
        public bool Noise { get; set; }
        public double Volume { get; set; }
        public PowerLevel Power { get; set; }
    }

    public enum PowerLevel
    {
        Hight,
        Low
    }
}
