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
        private bool IsMicrophoneStarted = false;
        private bool IsToneStarted = false;
        private ToneProvider _tone;
        private Guid _SenderId = Guid.NewGuid();

        public AudioReaderAndSender(IMicrophone microphone, IDataTransmitter transmitter,
            IDataAsByteConverter<DataModel> converter, ToneProvider tone)
        {
            _tone = tone;
            _tone.OnDataAvailable += Microphone_OnDataAvailable;
            _microphone = microphone;
            _microphone.OnDataAvailable += Microphone_OnDataAvailable;
            _transmitter = transmitter;
            _converter = converter;
        }

        private void Microphone_OnDataAvailable(object sender, DataEventArgs e)
        {
            var dataModel = new DataModel()
            {
                Guid = _SenderId, // TODO: unique guid for application
                RadioModel = _model,
                RawAudioSample = e.Data
            };

            var bytes = _converter.ConvertToBytes(dataModel);

            var result = _transmitter.Write(bytes);
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
            if (IsMicrophoneStarted)
                return;

            IsMicrophoneStarted = true;
            _microphone.StartListen();
        }

        public void StopListenMicrophone()
        {
            if (!IsMicrophoneStarted)
                return;

            IsMicrophoneStarted = false;
            _microphone.StopListen();
        }

        public void StartListenTone()
        {
            if (IsToneStarted)
                return;

            IsToneStarted = true;
            _tone.StartListen();
        }

        public void StopListenTone()
        {
            if (!IsToneStarted)
                return;

            IsToneStarted = false;
            _tone.StopListen();
        }

        public void Dispose()
        {
            _microphone.Dispose();
            _transmitter.Dispose();
        }
    }
}
