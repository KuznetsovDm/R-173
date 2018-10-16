using System;

namespace P2PMulticastNetwork.Model
{
    [Serializable]
    public struct SendableRadioModel
    {
        public int Frequency { get; set; }
        public bool PlayTone { get; set; }
    }
}
