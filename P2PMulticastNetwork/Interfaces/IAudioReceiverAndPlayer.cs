namespace P2PMulticastNetwork.Interfaces
{
    public interface IAudioReceiverAndPlayer<T> : IPipeline<T>
    {
        void Start();
        void Stop();
    }
}
