using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using P2PMulticastNetwork.Model;
using R_173.Interfaces;
using RadioPipeline;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace R_173.BL.Handlers
{
    public class RemoteToneHandler : IInvoker
    {
        private WaveFormat _format;
        private ToneProvider _toneProvider;
        private MixingSampleProvider _mixer;
        private static ConcurrentDictionary<Guid, PlayerSampleToken> _providers;
        private static TimeSpan timeDifferencePlaying = TimeSpan.FromMilliseconds(1500);

        public RemoteToneHandler(WaveFormat format, ToneProvider provider, MixingSampleProvider mixer)
        {
            _format = format;
            _toneProvider = provider;
            _mixer = mixer;
            if (_providers == null)
                _providers = new ConcurrentDictionary<Guid, PlayerSampleToken>();
        }

        public async Task Invoke(DataModel model, PipelineDelegate<DataModel> next)
        {
            if (_providers.ContainsKey(model.Guid) && model.RadioModel.Tone)
            {
                var token = _providers[model.Guid];
                var current = DateTime.UtcNow.TimeOfDay;
                if ((current - token.TimeStamp) > timeDifferencePlaying)
                {
                    _mixer.RemoveMixerInput(token.Provider);

                    var updatedSample = _toneProvider.CreateSampleProvider();
                    _providers[model.Guid] = new PlayerSampleToken
                    {
                        Provider = updatedSample,
                        TimeStamp = current
                    };

                    _mixer.AddMixerInput(updatedSample);
                }
            }
            else if (model.RadioModel.Tone)
            {
                var sample = _toneProvider.CreateSampleProvider();
                var timeStamp = DateTime.UtcNow.TimeOfDay;
                var token = new PlayerSampleToken
                {
                    Provider = sample,
                    TimeStamp = timeStamp
                };
                _providers[model.Guid] = token;

                _mixer.AddMixerInput(token.Provider);
            }
            await next.Invoke(model);
        }
    }

    public struct PlayerSampleToken
    {
        public ISampleProvider Provider { get; set; }
        public TimeSpan TimeStamp { get; set; }
    }
}
