using NAudio.Wave;
using System;

namespace R_173.BL
{
    public class EmptyWrapperWaveIn : IWaveIn
    {
        public EmptyWrapperWaveIn(WaveFormat format)
        {
            WaveFormat = format;
        }

        public WaveFormat WaveFormat
        {
            get;
            set;
        }

        public event EventHandler<WaveInEventArgs> DataAvailable;
        public event EventHandler<StoppedEventArgs> RecordingStopped;

        public void Dispose()
        {
        }

        public void StartRecording()
        {
        }

        public void StopRecording()
        {
        }
    }
}
