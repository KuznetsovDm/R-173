using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using R_173.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace R_173.BL
{
    public class NoiseProvider : ISampleProvider, IGlobalNoiseController
    {
        private SignalGenerator _signalrGenerator;

        public NoiseProvider()
        {
            _signalrGenerator = new SignalGenerator { Type = SignalGeneratorType.White };
        }

        public WaveFormat WaveFormat => _signalrGenerator.WaveFormat;

        public int Read(float[] buffer, int offset, int count)
        {
            if (_useRead)
                return _signalrGenerator.Read(buffer, offset, count);
            return 0;
        }

        public void Play()
        {
            _useRead = true;
        }

        public void Stop()
        {
            _useRead = false;
        }

        private bool _useRead = false;

        public double Volume { get => _signalrGenerator.Gain; set => _signalrGenerator.Gain = value; }
    }
}
