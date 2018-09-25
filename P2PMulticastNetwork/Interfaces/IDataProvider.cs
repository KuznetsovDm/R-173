using P2PMulticastNetwork.Model;
using System;

namespace P2PMulticastNetwork.Interfaces
{
    public interface IDataProvider : IDisposable
    {
        event EventHandler<DataEventArgs> OnDataAvaliable;
        void Start();
        void Stop();
    }
}
