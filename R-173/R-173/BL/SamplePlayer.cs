using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using R_173.BL.Utils;
using R_173.Interfaces;
using System;

namespace R_173.BL
{
    public class SamplePlayer : ISamplePlayer
    {
        private readonly IWavePlayer _player;
        private readonly MixingSampleProvider _mixer;
        private readonly VolumeSampleProvider _volumeFilter;

	    public SamplePlayer(WaveFormat format, IWavePlayer player)
        {
            _mixer = new MixingSampleProvider(format);
            _volumeFilter = new VolumeSampleProvider(_mixer);

	        _player = player;
            _player.Init(_volumeFilter);
        }

        public float Volume
        {
            get => _volumeFilter.Volume;
            set => _volumeFilter.Volume = VolumeSamplesHelper.LogVolumeApproximation(value);
        }

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
