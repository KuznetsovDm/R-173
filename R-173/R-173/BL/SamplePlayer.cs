using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using R_173.Interfaces;
using System;

namespace R_173.BL
{
    public class SamplePlayer : ISamplePlayer
    {
        private WaveOut _player;
        private MixingSampleProvider _mixer;
        private VolumeSampleProvider _volumeFilter;
        private WaveFormat _format;

        public SamplePlayer(WaveFormat format)
        {
            _mixer = new MixingSampleProvider(format);
            _volumeFilter = new VolumeSampleProvider(_mixer);
            _format = format;

            _player = new WaveOut();
            _player.Init(_volumeFilter);
        }

        public float Volume { get => _volumeFilter.Volume; set => _volumeFilter.Volume = (float)Math.Pow(value * 10, 2f); }

        public void Add(ISampleProvider provider)
        {
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));
            _mixer.AddMixerInput(provider);
        }

        public void Dispose()
        {
            _player.Dispose();
        }

        public void Play()
        {
            _player.Play();
        }

        public void Stop()
        {
            _player.Stop();
        }
    }
}
