using System;

namespace P2PMulticastNetwork.Interfaces
{
    public interface IAudioReaderAndSender<T> : IPipeline<T>, IDisposable
    {
        void StartListenMicrophone();
        void StopListenMicrophone();
        void StartListenTone();
        void StopListenTone();
    }
}
