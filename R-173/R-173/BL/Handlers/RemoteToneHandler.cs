using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using P2PMulticastNetwork.Model;
using R_173.Interfaces;
using RadioPipeline;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R_173.BL.Handlers
{
    public class RemoteToneHandler : IInvoker
    {
        private WaveFormat _format;
        private ToneProvider _toneProvider;
        private MixingSampleProvider _mixer;
        private static ConcurrentDictionary<Guid, ISampleProvider> _providers;
        private static TimeSpan timeDifferencePlaying = TimeSpan.FromSeconds(2);

        public RemoteToneHandler(WaveFormat format, ToneProvider provider, MixingSampleProvider mixer)
        {
            _format = format;
            _toneProvider = provider;
            _mixer = mixer;
            if (_providers == null)
                _providers = new ConcurrentDictionary<Guid, ISampleProvider>();
        }

        public async Task Invoke(DataModel model, PipelineDelegate<DataModel> next)
        {
            //todo: memory.
            //if (_providers.ContainsKey(model.Guid))
            //{

            //}
            //else
            //{

            //}
            if (model.RadioModel.Tone)
            {
                _mixer.AddMixerInput(_toneProvider.GetSampleProvider());
            }
            await next.Invoke(model);
        }
    }

    public class PlayerSampleToken
    {
        public ISampleProvider Provider { get; set; }
        public TimeSpan TimeStamp { get; set; }
    }
}
