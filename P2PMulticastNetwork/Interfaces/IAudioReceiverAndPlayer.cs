using System;

namespace P2PMulticastNetwork.Interfaces
{
    public interface IAudioReceiverAndPlayer<T> : IPipeline<T>, IDisposable
    {
        void Start();
        void Stop();
    }
}
