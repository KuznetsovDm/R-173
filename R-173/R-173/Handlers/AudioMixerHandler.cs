﻿using P2PMulticastNetwork.Model;
using R_173.Extensions;
using RadioPipeline;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace R_173.Handlers
{
    public class AudioMixerHandler : IInvoker
    {
        private static ConcurrentDictionary<Guid, BufferedWaveProvider> _buffers;
        private MixingSampleProvider _mixer;
        private WaveFormat _format;

        public AudioMixerHandler(WaveFormat format, MixingSampleProvider mixer)
        {
            _mixer = mixer;
            //todo: check guid comparation speed
            if(_buffers == null)
                _buffers = new ConcurrentDictionary<Guid, BufferedWaveProvider>();
            _format = format;
        }

        public async Task Invoke(DataModel model, PipelineDelegate<DataModel> next)
        {
            var buffer = _buffers.GetOrAdd(model.Guid, (guid) =>
            {
                var dictionaryBuffer = new BufferedWaveProvider(_format);
                _mixer.AddMixerInput(dictionaryBuffer);
                return dictionaryBuffer;
            });

            buffer.AddSamples(model.RawAudioSample, 0, model.RawAudioSample.Length);
            await next.Invoke(model);
        }
    }
}
