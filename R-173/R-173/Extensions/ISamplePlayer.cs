using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace R_173.Extensions
{
    public interface ISamplePlayer : IDisposable
    {
        void Add(ISampleProvider provider);
        void Play();
        void Stop();
    }

    public class SamplePlayer : ISamplePlayer
    {
        private WaveOut _player;
        private MixingSampleProvider _mixer;
        private WaveFormat _format;

        public SamplePlayer(WaveFormat format)
        {
            _mixer = new MixingSampleProvider(format);
            _format = format;

            _player = new WaveOut();
            _player.Init(_mixer);
        }

        public void Add(ISampleProvider provider)
        {
            if(provider == null)
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
