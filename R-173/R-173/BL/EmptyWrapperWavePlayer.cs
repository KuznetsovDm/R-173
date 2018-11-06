using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace R_173.BL
{
    public class EmptyWrapperWavePlayer : IWavePlayer
    {
        public PlaybackState PlaybackState => PlaybackState.Stopped;

        public float Volume
        {
            get => 0;
            set { }
        }

        public event EventHandler<StoppedEventArgs> PlaybackStopped;

        public void Dispose()
        {
        }

        public void Init(IWaveProvider waveProvider)
        {
        }

        public void Pause()
        {
        }

        public void Play()
        {
        }

        public void Stop()
        {
        }
    }

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
