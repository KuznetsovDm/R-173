using System;

namespace P2PMulticastNetwork.Interfaces
{
    public interface IDataProvider : IDisposable
    {
        void OnDataAwaliable(Action<byte[]> action);
    }
}
