using P2PMulticastNetwork.Model;
using System;

namespace R_173.Interfaces
{
    public interface IMicrophone : IDisposable
    {
        event EventHandler<ByteDataEventArgs> OnDataAvailable;
        void StartListen();
        void StopListen();
    }
}
