﻿using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace R_173.BL
{
    public class LocalToneController
    {
        private MixingSampleProvider _mixer;
        private ToneProvider _toneProvider;
        private ISampleProvider _sample;

        public LocalToneController(MixingSampleProvider mixer, ToneProvider provider)
        {
            _mixer = mixer;
            _toneProvider = provider;
        }

        public void StartPlayTone()
        {
            _sample = _toneProvider.CreateSampleProvider();
            _mixer.AddMixerInput(_sample);
        }

        public void StopPlayTone()
        {
            _mixer.RemoveMixerInput(_sample);
        }
    }
}
