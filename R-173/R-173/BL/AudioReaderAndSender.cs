using P2PMulticastNetwork.Interfaces;
using P2PMulticastNetwork.Model;
using R_173.Interfaces;
using System;

namespace R_173.BL
{
    public class AudioReaderAndSender : IAudioReaderAndSender<SendableRadioModel>
    {
        private IMicrophone _microphone;
        private SendableRadioModel _model;
        private IDataTransmitter _transmitter;
        private IDataAsByteConverter<DataModel> _converter;
        private DataCompressor _compressor;
        private LocalToneController _localToneController;
        private bool _isMicrophoneStarted = false;
        private bool _isToneStarted = false;
        private ToneProvider _tone;
        private Guid _SenderId = Guid.NewGuid();

        public AudioReaderAndSender(IMicrophone microphone,
            IDataTransmitter transmitter,
            IDataAsByteConverter<DataModel> converter,
            ToneProvider tone,
            DataCompressor compressor,
            LocalToneController localToneController)
        {
            _tone = tone;
            _microphone = microphone;
            _microphone.OnDataAvailable += OnSendDataAvailable;
            _transmitter = transmitter;
            _converter = converter;
            _compressor = compressor;
            _localToneController = localToneController;
        }

        private void OnSendDataAvailable(object sender, DataEventArgs e)
        {
            var dataModel = new DataModel()
            {
                Guid = _SenderId,
                RadioModel = _model,
                RawAudioSample = e.Data,
            };

            var bytes = _converter.ConvertToBytes(dataModel);

            var compressed = _compressor.Compress(bytes);

            var result = _transmitter.Write(compressed);
            if (result.IsFailure)
            {
                // TODO: logger
            }
        }

        public void SetModel(SendableRadioModel model)
        {
            _model = model;
        }

        public void StartListenMicrophone()
        {
            if (_isMicrophoneStarted)
                return;

            _isMicrophoneStarted = true;
            _microphone.StartListen();
        }

        public void StopListenMicrophone()
        {
            if (!_isMicrophoneStarted)
                return;

            _isMicrophoneStarted = false;
            _microphone.StopListen();
        }

        public void StartListenTone()
        {
            if (_isToneStarted)
                return;

            _isToneStarted = true;
            //todo:
            _localToneController.StartPlayTone();
            //_tone.StartListen();
        }

        public void StopListenTone()
        {
            if (!_isToneStarted)
                return;

            _isToneStarted = false;
            //todo: 
            _localToneController.StopPlayTone();
            //_tone.StopListen();
        }

        public void Dispose()
        {
            _microphone.Dispose();
            _transmitter.Dispose();
        }
    }
}
