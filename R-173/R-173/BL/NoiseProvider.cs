using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using R_173.Interfaces;

namespace R_173.BL
{
    public class NoiseProvider : ISampleProvider, IGlobalNoiseController
    {
        private readonly SignalGenerator _signalrGenerator;

        public NoiseProvider(WaveFormat format)
        {
            _signalrGenerator = new SignalGenerator(format.SampleRate, format.Channels) { Type = SignalGeneratorType.White };
        }

        public WaveFormat WaveFormat => _signalrGenerator.WaveFormat;

        public int Read(float[] buffer, int offset, int count)
        {
	        return _useRead ? _signalrGenerator.Read(buffer, offset, count) : 0;
        }

        public void Play()
        {
            _useRead = true;
        }

        public void Stop()
        {
            _useRead = false;
        }

        private bool _useRead;

        public double Volume { get => _signalrGenerator.Gain; set => _signalrGenerator.Gain = value; }
    }
}
