using CSharpFunctionalExtensions;
using System;
using System.Threading.Tasks;

namespace P2PMulticastNetwork.Interfaces
{
    public interface IDataReceiver : IDisposable
    {
        Task<Result<byte[]>> Receive();
    }
}
