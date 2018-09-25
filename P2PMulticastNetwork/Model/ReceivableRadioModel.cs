using System;

namespace P2PMulticastNetwork.Model
{
    [Serializable]
    public struct ReceivableRadioModel
    {
        public int Frequency { get; set; }
        public bool Noise { get; set; }
        public double Volume { get; set; }
    }
}
