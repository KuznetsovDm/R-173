using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace R_173.BL.Handlers
{
    public static class AudioVolumeSamplesHelper
    {
        public static void SetVolume(byte[] buffer, float volume)
        {
            float[] floatBuffer = new WaveBuffer(buffer).FloatBuffer;
            int floats = buffer.Length / sizeof(float);
            for (int i = 0; i < floats; i++)
            {
                floatBuffer[i] *= 4;
            }
        }
    }
}
