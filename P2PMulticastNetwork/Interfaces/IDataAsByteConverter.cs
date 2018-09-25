namespace P2PMulticastNetwork.Interfaces
{
    public interface IDataAsByteConverter<T>
    {
        T ConvertFrom(byte[] bytes);
        byte[] ConvertToBytes(T data);
    }
}
