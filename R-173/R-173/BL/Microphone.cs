using NAudio.Wave;
using P2PMulticastNetwork.Model;
using R_173.Interfaces;
using System;

namespace R_173.BL
{
    public class Microphone : IMicrophone
    {
        private IWaveIn _audioListener;

        public Microphone(WaveFormat format, IWaveIn waveIn)
        {
            _audioListener = waveIn;
            _audioListener.WaveFormat = format;
            _audioListener.DataAvailable += (obj, args) =>
            {
                var bytes = new byte[args.BytesRecorded];
                Array.Copy(args.Buffer, bytes, args.BytesRecorded);
                OnDataAvailable?.Invoke(this, new DataEventArgs()
                {
                    Data = bytes
                });
            };
        }

        public event EventHandler<DataEventArgs> OnDataAvailable;

        public void Dispose()
        {
            _audioListener.Dispose();
        }

        public void StartListen()
        {
            _audioListener.StartRecording();
        }

        public void StopListen()
        {
            _audioListener.StopRecording();
        }
    }
}
