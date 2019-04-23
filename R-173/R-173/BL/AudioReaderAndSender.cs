using System;
using P2PMulticastNetwork.Interfaces;
using P2PMulticastNetwork.Model;
using R_173.BE;
using R_173.Interfaces;

namespace R_173.BL
{
    public class AudioReaderAndSender : IAudioReaderAndSender<SendableRadioModel>
    {
        private readonly IMicrophone _microphone;
        private SendableRadioModel _model;
        private readonly IDataTransmitter _transmitter;
        private readonly IDataAsByteConverter<DataModel> _converter;
        private readonly DataCompressor _compressor;
        private readonly LocalToneController _localToneController;
        private bool _isMicrophoneStarted;
        private bool _isToneStarted;
        private readonly Guid _senderId;
	    private readonly INetworkTaskManager _networkTaskManager;

        public AudioReaderAndSender(IMicrophone microphone,
            IDataTransmitter transmitter,
            IDataAsByteConverter<DataModel> converter,
            DataCompressor compressor,
            LocalToneController localToneController,
            RadioSettings settings, 
	        INetworkTaskManager networkTaskManager)
        {
            _senderId = settings.NetworkToken;
            _microphone = microphone;
            _microphone.OnDataAvailable += OnSendDataAvailable;
            _transmitter = transmitter;
            _converter = converter;
            _compressor = compressor;
            _localToneController = localToneController;
	        _networkTaskManager = networkTaskManager;
        }

        private void OnSendDataAvailable(object sender, ByteDataEventArgs e)
        {
            var dataModel = new DataModel
            {
                Guid = _senderId,
                RadioModel = _model,
                RawAudioSample = e.Data,
				NetworkTask = _networkTaskManager.CurrentNetworkTask
            };

            var bytes = _converter.ConvertToBytes(dataModel);

            var compressed = _compressor.Compress(bytes);

            var result = _transmitter.Write(compressed);
            if (result.IsFailure)
            {
                SimpleLogger.Log(result.Error);
            }
        }

        public void SetModel(SendableRadioModel model)
        {
            _model = model;
        }

        public void StartListenMicrophone()
        {
            try
            {
                if (_isMicrophoneStarted)
                    return;

                _isMicrophoneStarted = true;
                _microphone.StartListen();
            }
	        catch (Exception)
	        {
		        // ignored
	        }
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
            try
            {
                if (_isToneStarted)
                    return;

                _isToneStarted = true;
                _localToneController.StartPlayTone();
            }
	        catch (Exception)
	        {
		        // ignored
	        }
        }

        public void StopListenTone()
        {
            if (!_isToneStarted)
                return;

            _isToneStarted = false;
            _localToneController.StopPlayTone();
        }

        public void Dispose()
        {
            _microphone.Dispose();
            _transmitter.Dispose();
        }
    }
}
