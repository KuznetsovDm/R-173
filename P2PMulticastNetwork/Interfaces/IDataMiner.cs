using System;

namespace P2PMulticastNetwork.Interfaces
{
    public interface IDataMiner : IDisposable
    {
        void OnDataAwaliable(Action<byte[]> action);
        void Start();
        void Stop();
    }
}
