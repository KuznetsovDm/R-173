using P2PMulticastNetwork.Model;
using System;

namespace P2PMulticastNetwork.Interfaces
{
    public interface IDataProvider<T> : IDisposable
    {
        event EventHandler<DataEventArgs<T>> OnDataAvaliable;
        void Start();
        void Stop();
    }
}
