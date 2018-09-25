namespace P2PMulticastNetwork.Interfaces
{
    public interface IPipeline<T>
    {
        void SetFilter(T filter);
    }
}
