using P2PMulticastNetwork.Interfaces;
using P2PMulticastNetwork.Model;
using R_173.BL.Handlers;
using R_173.BL.Utils;
using R_173.Handlers;
using R_173.Interfaces;
using RadioPipeline;
using System;
using System.Threading.Tasks;
using System.IO.Compression;
using System.Diagnostics;

namespace R_173.BL
{
    public class AudioReceiverAndPlayer : IAudioReceiverAndPlayer<ReceivableRadioModel>
    {
        public const float LowPowerLevelVolume = 0.5f;
        private ReceivableRadioModel _model;
        private IDataProvider _provider;
        private ISamplePlayer _player;
        private IDataAsByteConverter<DataModel> _converter;
        private IDataProcessingBuilder _builder;
        private PipelineDelegate<DataModel> _pipeline;
        private bool IsPlayerStarted = false;
        private IGlobalNoiseController _noise;
        private DataCompressor _compressor;

        public AudioReceiverAndPlayer(IDataProvider provider, ISamplePlayer player,
            IDataAsByteConverter<DataModel> converter, IDataProcessingBuilder builder,
            IGlobalNoiseController globalNoise,
            DataCompressor compressor)
        {
            _provider = provider;
            _player = player;
            _converter = converter;
            _builder = builder;
            Build();
            _provider.OnDataAvaliable += Provider_OnDataAvaliable;
            _noise = globalNoise;
            _compressor = compressor;
        }

        private void Provider_OnDataAvaliable(object sender, DataEventArgs e)
        {
            try
            {
                var decompressed = _compressor.Decompress(e.Data);
                var model = _converter.ConvertFrom(decompressed);
                _pipeline.Invoke(model);
            }
            catch (Exception ex)
            {
                Debug.Write($"An exception in {nameof(AudioReceiverAndPlayer)} {ex.Message}.");
            }
        }

        private void Build()
        {
            _pipeline = _builder.Use(async (model, next) =>
                                {
                                    bool isInListeningRange = Math.Abs(model.RadioModel.Frequency - _model.Frequency) < _model.FrequencyListeningRange;
                                    bool isInValidRadioRange = _model.MinFrequency <= model.RadioModel.Frequency && model.RadioModel.Frequency <= _model.MaxFrequency;
                                    if (isInValidRadioRange && isInListeningRange)
                                        await next.Invoke(model);
                                })
                                .Use(async (model, next) =>
                                {
                                    if (_model.Power == PowerLevel.Low)
                                    {
                                        var volume = VolumeSamplesHelper.LogVolumeApproximation(LowPowerLevelVolume);
                                        VolumeSamplesHelper.SetVolume(model.RawAudioSample, volume);
                                    }
                                    await next.Invoke(model);
                                })
                                .Use(async (model, next) =>
                                {
                                    float deltaAbs = 1 - (float)Math.Abs(model.RadioModel.Frequency - _model.Frequency) / _model.FrequencyListeningRange;
                                    float volume = VolumeSamplesHelper.LogVolumeApproximation(deltaAbs);
                                    VolumeSamplesHelper.SetVolume(model.RawAudioSample, volume);
                                    await next.Invoke(model);
                                })
                                .UseMiddleware<RemoteToneHandler>()
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
        }

        public void Stop()
        {
            if (!IsPlayerStarted)
                return;

            IsPlayerStarted = false;
            _provider.Stop();
            _player.Stop();
            _noise.Stop();
        }

        public void Dispose()
        {
            _player.Dispose();
            _provider.Dispose();
        }
    }
}