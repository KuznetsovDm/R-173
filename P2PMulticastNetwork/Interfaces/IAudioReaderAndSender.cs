using System;

namespace P2PMulticastNetwork.Interfaces
{
    public interface IAudioReaderAndSender<T> : IPipeline<T>, IDisposable
    {
        void Start();
        void Stop();
    }
}
