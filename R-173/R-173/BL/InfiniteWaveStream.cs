using NAudio.Wave;

namespace R_173.BL
{
    public class InfiniteWaveStream : WaveStream
    {
        private readonly WaveStream _stream;

        public InfiniteWaveStream(WaveStream stream)
        {
            _stream = stream;
        }

        public override WaveFormat WaveFormat => _stream.WaveFormat;

        public override long Length => _stream.Length;

        public override long Position { get => _stream.Position; set => _stream.Position = value; }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (_stream.Position + count >= _stream.Length)
            {
                _stream.Position = 0;
            }
            return _stream.Read(buffer, offset, count);
        }
    }
}
