using NAudio.Wave;

namespace R_173.BL
{
    public class EmptySampleProvider : ISampleProvider
    {
        public EmptySampleProvider(WaveFormat format)
        {
            WaveFormat = format;
        }

        public WaveFormat WaveFormat { get; }

        public int Read(float[] buffer, int offset, int count)
        {
            return 0;
        }
    }
}
