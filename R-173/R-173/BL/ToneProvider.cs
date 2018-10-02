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

        public ToneProvider(WaveFormat format)
        {
            _format = format;
            //todo:
            _latencyInMs = 100;
            _tonGenerator = new SignalGenerator(_format.ConvertLatencyToByteSize(_latencyInMs),
                _format.Channels)
            { Type = SignalGeneratorType.Sin };
            _buffer = new WaveBuffer(format.SampleRate);
        }

        public event EventHandler<DataEventArgs> OnDataAvailable;

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void StartListen()
        {
            _tokenSource?.Dispose();

            _tokenSource = new CancellationTokenSource();
            _task = TaskEx.Run(async () => await ReceiveCycle(), _tokenSource.Token);
        }

        public async Task ReceiveCycle()
        {
            while (true)
            {
                await TaskEx.Delay(TimeSpan.FromMilliseconds(_latencyInMs));
                if (_tokenSource.Token.IsCancellationRequested)
                    return;

                var length = _buffer.FloatBuffer.Length / 4;
                _tonGenerator.Read(_buffer.FloatBuffer, 0, length);
                OnDataAvailable?.Invoke(this, new DataEventArgs { Data = _buffer.ByteBuffer });
            }
        }

        public void StopListen()
        {
            _tokenSource.Cancel();
            _tokenSource.Dispose();
        }
    }
}
