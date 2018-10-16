using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using P2PMulticastNetwork.Model;
using R_173.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace R_173.BL
{
    public class ToneProvider
    {
        private byte[] _rawTone;

        public ToneProvider(WaveFormat format)
        {
            _rawTone = Properties.Resources.RawTone;
        }

        public void Dispose()
        {
            _rawTone = null;
        }

        public ISampleProvider GetSampleProvider()
        {
            var stream = new MemoryStream(_rawTone);
            var reader = new Mp3FileReader(stream);
            return reader.ToSampleProvider();
        }
    }
}
