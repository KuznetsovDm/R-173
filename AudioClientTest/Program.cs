using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using P2PMulticastNetwork;
using P2PMulticastNetwork.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;

namespace AudioClientTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var noise = new SignalGenerator
            {
                Type = SignalGeneratorType.White,
                Gain = 0.1,
                Frequency = 123132,
            };

            var path = "1.mp3";
            if(args.Length > 0)
                path = args[0];

            var options = MulticastConnectionOptions.Create(useBind: false, exclusiveAddressUse: false, ipAddress: "225.0.0.0");

            var compressor = new DataCompressor();
            var guid = Guid.NewGuid();
            var converter = new DataModelConverter();

            var transmitter = new UdpMulticastConnection(options);
            using(var audioFile = new AudioFileReader(path))
            {
                var buffer = new byte[audioFile.WaveFormat.SampleRate];
                var bufferProvider = new BufferedWaveProvider(audioFile.WaveFormat);
                double d = audioFile.WaveFormat.AverageBytesPerSecond / audioFile.WaveFormat.SampleRate;
                var timer = new Timer((x)=>
                {
                    audioFile.Read(buffer, 0, buffer.Length);
                    var data = new DataModel
                    {
                        Guid = guid,
                        RawAudioSample = buffer,
                        RadioModel = new SendableRadioModel{ Frequency = 100 }
                    };
                    var converted = converter.ConvertToBytes(data);
                    var compressed = compressor.Compress(converted);
                    transmitter.Write(compressed);
                });

                timer.Change(TimeSpan.FromSeconds(0), TimeSpan.FromMilliseconds(1000 / (d)));
                while(true)
                    Thread.Sleep(1000);
                timer.Dispose();
            }
            Console.ReadKey();
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
