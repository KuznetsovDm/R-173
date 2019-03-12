using NAudio.Wave;
using System;

namespace R_173.BL.Utils
{
    public static class VolumeSamplesHelper
    {
        public static void SetVolume(byte[] buffer, float volume)
        {
            float[] floatBuffer = new WaveBuffer(buffer).FloatBuffer;
            int floats = buffer.Length / sizeof(float);
            for (int i = 0; i < floats; i++)
            {
                floatBuffer[i] *= volume;
            }
        }

        public static float LogVolumeApproximation(float value)
        {
            return (float)Math.Pow(value, Math.E);
        }
    }
}
