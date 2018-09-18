using System;

namespace P2PMulticastNetwork.Interfaces
{
    public interface IDataMiner : IDataProvider, IDisposable
    {
        void ReloadDataReceiver(IDataReceiver dataReceiver);
        void Start();
        void Stop();
    }
}
