using System.IO;
using System.IO.Compression;

namespace R_173.BL
{
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
            var bytes = new byte[4096];
            int cnt;
            while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
            {
                dest.Write(bytes, 0, cnt);
            }
        }

    }
}
