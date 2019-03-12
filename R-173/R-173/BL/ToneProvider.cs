using NAudio.Wave;
using System.IO;

namespace R_173.BL
{
    public class ToneProvider
    {
        private byte[] _rawTone;
        private readonly EmptySampleProvider _emptyProvider;

        public ToneProvider(WaveFormat format)
        {
            _rawTone = Properties.Resources.RawTone;
            _emptyProvider = new EmptySampleProvider(format);
        }

        public void Dispose()
        {
            _rawTone = null;
        }

        public ISampleProvider CreateSampleProvider()
        {
	        if (_rawTone == null) return _emptyProvider;

	        var stream = new MemoryStream(_rawTone);
	        var reader = new Mp3FileReader(stream);
	        return reader.ToSampleProvider();

        }

        public InfiniteWaveStream CreateInfiniteWaveStream()
        {
	        if (_rawTone == null) return null;

	        var stream = new MemoryStream(_rawTone);
	        var reader = new Mp3FileReader(stream);
	        return new InfiniteWaveStream(reader);
        }
    }
}
