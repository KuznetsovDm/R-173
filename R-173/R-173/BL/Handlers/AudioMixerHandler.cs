using P2PMulticastNetwork.Model;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using P2PMulticastNetwork;
using R_173.Interfaces;

namespace R_173.BL.Handlers
{
    public class AudioMixerHandler : IInvoker
    {
        private static ConcurrentDictionary<Guid, BufferedWaveProvider> _buffers;
        private readonly MixingSampleProvider _mixer;
        private readonly WaveFormat _format;

        public AudioMixerHandler(WaveFormat format, MixingSampleProvider mixer)
        {
            _mixer = mixer;
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
