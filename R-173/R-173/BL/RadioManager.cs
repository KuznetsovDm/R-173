using P2PMulticastNetwork.Interfaces;
using P2PMulticastNetwork.Model;
using R_173.Handlers;
using R_173.Interfaces;
using R_173.Models;
using R_173.SharedResources;
using R_173.ViewModels;

namespace R_173.BL
{
    public class RadioManager : IRadioManager
    {
        private RadioModel _radioModel;
        private IAudioReaderAndSender<SendableRadioModel> _reader;
        private IAudioReceiverAndPlayer<ReceivableRadioModel> _player;
        private readonly KeyboardHandler _keyboardHandler;

        public RadioManager(IAudioReaderAndSender<SendableRadioModel> reader,
            IAudioReceiverAndPlayer<ReceivableRadioModel> player, KeyboardHandler keyboardHandler)
        {
            _reader = reader;
            _player = player;
            _keyboardHandler = keyboardHandler;
        }

        public void SetModel(RadioModel radioModel)
        {
            _keyboardHandler.ActivateRadio(radioModel);

            if (_radioModel != null)
            {
                UnsubscribeEvents(_radioModel);
            }

            _radioModel = radioModel;

            if (radioModel == null)
                return;

            SubscribeEvents(radioModel);

            InitRadioManager(_radioModel);
        }

        #region Events
        private void VolumePRM_ValueChanged(object sender, ValueChangedEventArgs<double> e)
        {

        }

        private void Volume_ValueChanged(object sender, ValueChangedEventArgs<double> e)
        {
            _player.SetModel(GetReceivableRadioModelFromRadioModel(_radioModel));
        }

        private void TurningOn_ValueChanged(object sender, ValueChangedEventArgs<SwitcherState> e)
        {
            if (e.NewValue == SwitcherState.Enabled)
            {
                _player.Start();
            }
            else
            {
                _reader.StopListenMicrophone();
                _player.Stop();
            }
        }

        private void Tone_ValueChanged(object sender, ValueChangedEventArgs<SwitcherState> e)
        {
            if (e.NewValue == SwitcherState.Enabled)
                _reader.StartListenTone();
            else
                _reader.StopListenTone();
            System.Diagnostics.Trace.Write($"Tone: {e.NewValue}");
        }

        private void RightPuOa_ValueChanged(object sender, ValueChangedEventArgs<SwitcherState> e)
        {

        }

        private void RecordWork_ValueChanged(object sender, ValueChangedEventArgs<RecordWorkState> e)
        {

        }

        private void Power_ValueChanged(object sender, ValueChangedEventArgs<SwitcherState> e)
        {

        }

        private void Noise_ValueChanged(object sender, ValueChangedEventArgs<NoiseState> e)
        {
            _player.SetModel(GetReceivableRadioModelFromRadioModel(_radioModel));
        }

        private void LeftPuOa_ValueChanged(object sender, ValueChangedEventArgs<SwitcherState> e)
        {

        }

        private void Interference_ValueChanged(object sender, ValueChangedEventArgs<SwitcherState> e)
        {

        }

        private void Frequency_ValueChanged(object sender, ValueChangedEventArgs<string> e)
        {
            _reader.SetModel(GetSendableRadioModelFromRadioModel(_radioModel));
            _player.SetModel(GetReceivableRadioModelFromRadioModel(_radioModel));
        }
        #endregion

        private static SendableRadioModel GetSendableRadioModelFromRadioModel(RadioModel radioModel)
        {
            return new SendableRadioModel
            {
                Frequency = int.Parse(radioModel.Frequency.Value)
            };
        }

        private static ReceivableRadioModel GetReceivableRadioModelFromRadioModel(RadioModel radioModel)
        {
            return new ReceivableRadioModel
            {
                Frequency = int.Parse(radioModel.Frequency.Value),
                Noise = radioModel.Noise.Value == NoiseState.Minimum, // TODO: noise
                Volume = radioModel.Volume.Value
            };
        }

        private void SubscribeEvents(RadioModel radioModel)
        {
            radioModel.Frequency.ValueChanged += Frequency_ValueChanged;
            radioModel.Interference.ValueChanged += Interference_ValueChanged;
            radioModel.LeftPuOa.ValueChanged += LeftPuOa_ValueChanged;
            radioModel.Noise.ValueChanged += Noise_ValueChanged;
            radioModel.Power.ValueChanged += Power_ValueChanged;
            radioModel.RecordWork.ValueChanged += RecordWork_ValueChanged;
            radioModel.RightPuOa.ValueChanged += RightPuOa_ValueChanged;
            radioModel.Tone.ValueChanged += Tone_ValueChanged;
            radioModel.TurningOn.ValueChanged += TurningOn_ValueChanged;
            radioModel.Volume.ValueChanged += Volume_ValueChanged;
            radioModel.VolumePRM.ValueChanged += VolumePRM_ValueChanged;
            radioModel.Sending.ValueChanged += Sending_ValueChanged;
        }

        private void Sending_ValueChanged(object sender, ValueChangedEventArgs<SwitcherState> e)
        {
            if(e.NewValue == SwitcherState.Enabled)
                _reader.StartListenMicrophone();
            else
                _reader.StopListenMicrophone();
        }

        private void UnsubscribeEvents(RadioModel radioModel)
        {
            radioModel.Frequency.ValueChanged -= Frequency_ValueChanged;
            radioModel.Interference.ValueChanged -= Interference_ValueChanged;
            radioModel.LeftPuOa.ValueChanged -= LeftPuOa_ValueChanged;
            radioModel.Noise.ValueChanged -= Noise_ValueChanged;
            radioModel.Power.ValueChanged -= Power_ValueChanged;
            radioModel.RecordWork.ValueChanged -= RecordWork_ValueChanged;
            radioModel.RightPuOa.ValueChanged -= RightPuOa_ValueChanged;
            radioModel.Tone.ValueChanged -= Tone_ValueChanged;
            radioModel.TurningOn.ValueChanged -= TurningOn_ValueChanged;
            radioModel.Volume.ValueChanged -= Volume_ValueChanged;
            radioModel.VolumePRM.ValueChanged -= VolumePRM_ValueChanged;
            radioModel.Sending.ValueChanged -= Sending_ValueChanged;
        }

        private void InitRadioManager(RadioModel model)
        {
            _player.SetModel(GetReceivableRadioModelFromRadioModel(model));
            _reader.SetModel(GetSendableRadioModelFromRadioModel(model));

            if(model.TurningOn.Value == SwitcherState.Enabled)
            {
                _player.Start();
            }
            else
            {
                _reader.StopListenMicrophone();
                _player.Stop();
            }
        }
    }
}
