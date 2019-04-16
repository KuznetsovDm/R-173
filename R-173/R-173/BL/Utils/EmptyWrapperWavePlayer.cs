using System;
using NAudio.Wave;

namespace R_173.BL.Utils
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
}
