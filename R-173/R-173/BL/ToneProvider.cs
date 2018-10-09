using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using P2PMulticastNetwork.Model;
using R_173.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace R_173.BL
{
    public class ToneProvider
    {
        private WaveFormat _format;
        private int _latencyInMs;
        private SignalGenerator _tonGenerator;
        private Task _task;
        private CancellationTokenSource _tokenSource;
        private WaveBuffer _buffer;
        private bool _canRead = false;

        public ToneProvider(WaveFormat format)
        {
            _format = format;
            _latencyInMs = 100;
            var sampleRate = _format.ConvertLatencyToByteSize(_latencyInMs);
            _tonGenerator = new SignalGenerator(sampleRate, _format.Channels)
            { Type = SignalGeneratorType.Sin };
            _buffer = new WaveBuffer(sampleRate);
        }

        public event EventHandler<DataEventArgs> OnDataAvailable;

        public void Dispose()
        {
            _buffer?.Clear();
            _buffer = null;
        }

        public void StartListen()
        {
            _canRead = true;

            _tokenSource = new CancellationTokenSource();
            _task = TaskEx.Run(async () => await ReceiveCycle(), _tokenSource.Token);
        }

        public async Task ReceiveCycle()
        {
            while (true)
            {
                if (_tokenSource.Token.IsCancellationRequested || !_canRead)
                    return;
                if (_canRead)
                {
                    var length = _buffer.FloatBuffer.Length / 4;
                    _tonGenerator.Read(_buffer.FloatBuffer, 0, length);
                    OnDataAvailable?.Invoke(this, new DataEventArgs { Data = _buffer.ByteBuffer });
                }
                await TaskEx.Delay(TimeSpan.FromMilliseconds(_latencyInMs));
            }
        }

        public void StopListen()
        {
            _canRead = false;
            _tokenSource.Cancel();
        }
    }
}
