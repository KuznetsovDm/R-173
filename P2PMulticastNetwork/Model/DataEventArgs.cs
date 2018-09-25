using System;

namespace P2PMulticastNetwork.Model
{
    public class DataEventArgs : EventArgs
    {
        public byte[] Data { get; set; }
    }
}
