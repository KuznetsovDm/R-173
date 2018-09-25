using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace R_173.Interfaces
{
    interface IMicrophone : IDisposable
    {
        event EventHandler<WaveInEventArgs> OnDataAvailable;
        void StartListen();
        void StopListen();
    }
}
