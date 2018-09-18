namespace P2PMulticastNetwork.Interfaces
{
    public interface IAudioReaderAndSender<T> : IPipeline<T>
    {
        void Start();
        void Stop();
    }
}
