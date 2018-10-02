using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace R_173.Interfaces
{
    public interface ISamplePlayer : IDisposable
    {
        void Add(ISampleProvider provider);
        void Play();
        void Stop();
        float Volume { get; set; }
    }
}
