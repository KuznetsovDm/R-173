using NAudio.Wave;
using R_173.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace R_173.BL
{
    public class Microphone : IMicrophone
    {
        private WaveIn _audioListener;

        public Microphone()
        {
            _audioListener = new WaveIn();
            _audioListener.DataAvailable += (obj, args) => OnDataAvailable?.Invoke(this, args);
        }

        public event EventHandler<WaveInEventArgs> OnDataAvailable;

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
