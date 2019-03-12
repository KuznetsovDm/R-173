namespace P2PMulticastNetwork.Model
{
    public struct ReceivableRadioModel
    {
        public int Frequency { get; set; }
        public bool Noise { get; set; }
        public double Volume { get; set; }
        public PowerLevel Power { get; set; }
        public int FrequencyListeningRange { get; set; }
        public int MinFrequency { get; set; }
        public int MaxFrequency { get; set; }
    }

    public enum PowerLevel
    {
        Hight,
        Low
    }
}
