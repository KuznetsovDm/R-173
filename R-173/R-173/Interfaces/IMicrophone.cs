using NAudio.Wave;
using P2PMulticastNetwork.Model;
using R_173.SharedResources;
using System;

namespace R_173.Interfaces
{
    public interface IMicrophone : IDisposable
    {
        event EventHandler<DataEventArgs> OnDataAvailable;
        void StartListen();
        void StopListen();
    }
}
