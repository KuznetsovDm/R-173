using P2PMulticastNetwork.Interfaces;
using P2PMulticastNetwork.Model;
using R_173.Handlers;
using R_173.Interfaces;
using RadioPipeline;
using System;
using System.Threading.Tasks;

namespace R_173.BL
{
    public class AudioReceiverAndPlayer : IAudioReceiverAndPlayer<ReceivableRadioModel>
    {
        public const int FrequencyRange = 100; // TODO: make constatnts
        private ReceivableRadioModel _model;
        private IDataProvider _provider;
        private ISamplePlayer _player;
        private IDataAsByteConverter<DataModel> _converter;
        private IDataProcessingBuilder _builder;
        private PipelineDelegate<DataModel> _pipeline;
        private bool IsPlayerStarted = false;
        private IGlobalNoiseController _noise;

        public AudioReceiverAndPlayer(IDataProvider provider, ISamplePlayer player,
            IDataAsByteConverter<DataModel> converter, IDataProcessingBuilder builder,
            IGlobalNoiseController globalNoise)
        {
            _provider = provider;
            _player = player;
            _converter = converter;
            _builder = builder;
            Build();
            _provider.OnDataAvaliable += Provider_OnDataAvaliable;
            _noise = globalNoise;
        }

        private void Provider_OnDataAvaliable(object sender, DataEventArgs e)
        {
            var model = _converter.ConvertFrom(e.Data);
            _pipeline.Invoke(model);
        }

        private void Build()
        {
            _pipeline = _builder.Use(async (model, next) =>
                                {
                                    //todo:
                                    //if ((model.RadioModel.Frequency - _model.Frequency) < FrequencyRange)
                                    await next.Invoke(model);
                                })
                                .UseMiddleware<AudioMixerHandler>()
                                .Build();
        }

        public void SetModel(ReceivableRadioModel model)
        {
            _model = model;
            _noise.Volume = model.Noise ? 1f : 0;
            _player.Volume = (float)model.Volume;
        }

        public void Start()
        {
            if (IsPlayerStarted)
                return;

            IsPlayerStarted = true;
            _provider.Start();
            _noise.Play();
            _player.Play();
            // TODO: noises start
        }

        public void Stop()
        {
            if (!IsPlayerStarted)
                return;

            IsPlayerStarted = false;
            _provider.Stop();
            _player.Stop();
            // TODO: noises stop
            _noise.Stop();
        }

        public void Dispose()
        {
            _player.Dispose();
            _provider.Dispose();
        }
    }
}