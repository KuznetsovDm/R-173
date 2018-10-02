using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using P2PMulticastNetwork.Model;
using R_173.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace R_173.BL
{
    public class ToneProvider
    {
        private WaveFormat _format;

        public ToneProvider(WaveFormat format)
        {
            _format = format;
        }

        public event EventHandler<DataEventArgs> OnDataAvailable;

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void StartListen()
        {
            throw new NotImplementedException();
        }

        public void StopListen()
        {
            throw new NotImplementedException();
        }
    }
}
