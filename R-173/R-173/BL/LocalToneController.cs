using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace R_173.BL
{
    public class LocalToneController
    {
        private readonly MixingSampleProvider _mixer;
	    private readonly ISampleProvider _sample;

        public LocalToneController(MixingSampleProvider mixer, ToneProvider provider)
        {
	        _mixer = mixer;
	        _sample = provider.CreateInfiniteWaveStream().ToSampleProvider();
        }

        public void StartPlayTone()
        {
            _mixer.AddMixerInput(_sample);
        }

        public void StopPlayTone()
        {
            _mixer.RemoveMixerInput(_sample);
        }
    }
}
