namespace P2PMulticastNetwork.Interfaces
{
    public interface IPipeline<in T>
    {
        void SetModel(T model);
    }
}
