using CSharpFunctionalExtensions;
using System;

namespace P2PMulticastNetwork.Interfaces
{
    public interface IDataTransmitter : IDisposable
    {
        Result Write(byte[] data);
    }
}
