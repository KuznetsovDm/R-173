using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using NAudio.Codecs;
using NAudio.Wave;

namespace R_173.BL
{
    public class BufferedWaveCompressor
    {
        private readonly G722CodecState _encodeState;
        private readonly G722CodecState _decodeState;
        private readonly G722Codec _codec;

        public BufferedWaveCompressor()
        {
            int rate = 64000;
            _encodeState = new G722CodecState(rate, G722Flags.None);
            _decodeState = new G722CodecState(rate, G722Flags.None);
            _codec = new G722Codec();
        }

        public byte[] Encode(byte[] data, int offset, int length)
        {
            if (offset != 0)
            {
                throw new ArgumentException("G722 does not yet support non-zero offsets");
            }
            WaveBuffer wb = new WaveBuffer(data);
            int encodedLength = length / 4;
            byte[] outputBuffer = new byte[encodedLength];
            int encoded = _codec.Encode(_encodeState, outputBuffer, wb.ShortBuffer, length / 2);
            return outputBuffer;
        }

        public byte[] Decode(byte[] data, int offset, int length)
        {
            if (offset != 0)
            {
                throw new ArgumentException("G722 does not yet support non-zero offsets");
            }
            int decodedLength = length * 4;
            byte[] outputBuffer = new byte[decodedLength];
            WaveBuffer wb = new WaveBuffer(outputBuffer);
            int decoded = _codec.Decode(_decodeState, wb.ShortBuffer, data, length);
            Debug.Assert(decodedLength == decoded * 2);  // because decoded is a number of samples
            return outputBuffer;
        }
    }

    public class DataCompressor
    {
        public byte[] Compress(byte[] bytes)
        {
            using (var stream = new MemoryStream(bytes))
            using (var outStream = new MemoryStream())
            {
                using (var compressor = new GZipStream(outStream, CompressionMode.Compress))
                {
                    CopyTo(stream, compressor);
                }
                return outStream.ToArray();
            }
        }

        public byte[] Decompress(byte[] bytes)
        {
            using (var stream = new MemoryStream(bytes))
            using (var outStream = new MemoryStream())
            {
                using (var decompressor = new GZipStream(stream, CompressionMode.Decompress))
                {
                    CopyTo(decompressor, outStream);
                }
                return outStream.ToArray();
            }
        }

        public static void CopyTo(Stream src, Stream dest)
        {
            byte[] bytes = new byte[4096];
            int cnt;
            while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
            {
                dest.Write(bytes, 0, cnt);
            }
        }

    }
}
