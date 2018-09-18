using System;

namespace R_173.Models
{
    [Serializable]
    public struct ReceivableRadioModel
    {
        public int Frequency { get; set; }
        public bool Noise { get; set; }
        public double Volume { get; set; }
    }
}
