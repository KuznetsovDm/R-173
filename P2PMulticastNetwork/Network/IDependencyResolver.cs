namespace P2PMulticastNetwork
{
    public interface IDependencyResolver
    {
        T GetService<T>();
    }
}