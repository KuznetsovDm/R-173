using P2PMulticastNetwork.Interfaces;
using P2PMulticastNetwork.Model;
using R_173.Handlers;
using R_173.Interfaces;
using R_173.Models;
using R_173.SharedResources;

namespace R_173.BL
{
    public class RadioManager : IRadioManager
    {
        private RadioModel _radioModel;
        private readonly IAudioReaderAndSender<SendableRadioModel> _reader;
        private readonly IAudioReceiverAndPlayer<ReceivableRadioModel> _player;
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
            if (!CheckTurningOn())
                return;

            _player.SetModel(GetReceivableRadioModelFromRadioModel(_radioModel));
        }

        private void TurningOn_ValueChanged(object sender, ValueChangedEventArgs<SwitcherState> e)
        {
            if (!CheckTurningOn())
                return;

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
            if (!CheckTurningOn())
                return;

            if (e.NewValue == SwitcherState.Enabled)
                _reader.StartListenTone();
            else
                _reader.StopListenTone();
        }

        private void RecordWork_ValueChanged(object sender, ValueChangedEventArgs<RecordWorkState> e)
        {

        }

        private void Power_ValueChanged(object sender, ValueChangedEventArgs<PowerState> e)
        {

        }

        private void Noise_ValueChanged(object sender, ValueChangedEventArgs<NoiseState> e)
        {
            if (!CheckTurningOn())
                return;

            _player.SetModel(GetReceivableRadioModelFromRadioModel(_radioModel));
        }

        private void Interference_ValueChanged(object sender, ValueChangedEventArgs<SwitcherState> e)
        {

        }

        private void FrequencyNumber_ValueChanged(object sender, ValueChangedEventArgs<int> e)
        {
            if (!CheckTurningOn())
                return;

            _reader.SetModel(GetSendableRadioModelFromRadioModel(_radioModel));
            _player.SetModel(GetReceivableRadioModelFromRadioModel(_radioModel));
        }

        private void Sending_ValueChanged(object sender, ValueChangedEventArgs<SwitcherState> e)
        {
            if (!CheckTurningOn())
                return;

            if (e.NewValue == SwitcherState.Enabled)
                _reader.StartListenMicrophone();
            else
                _reader.StopListenMicrophone();
        }

        #endregion

        private static SendableRadioModel GetSendableRadioModelFromRadioModel(RadioModel radioModel)
        {
            return new SendableRadioModel
            {
                Frequency = radioModel.WorkingFrequencies[radioModel.FrequencyNumber.Value]
            };
        }

        private static ReceivableRadioModel GetReceivableRadioModelFromRadioModel(RadioModel radioModel)
        {
            return new ReceivableRadioModel
            {
                Frequency = radioModel.WorkingFrequencies[radioModel.FrequencyNumber.Value],
                Noise = radioModel.Noise.Value == NoiseState.Minimum,
                Volume = radioModel.Volume.Value
            };
        }

        private void SubscribeEvents(RadioModel radioModel)
        {
            radioModel.FrequencyNumber.ValueChanged += FrequencyNumber_ValueChanged;
            radioModel.Interference.ValueChanged += Interference_ValueChanged;
            radioModel.Noise.ValueChanged += Noise_ValueChanged;
            radioModel.Power.ValueChanged += Power_ValueChanged;
            radioModel.RecordWork.ValueChanged += RecordWork_ValueChanged;
            radioModel.Tone.ValueChanged += Tone_ValueChanged;
            radioModel.TurningOn.ValueChanged += TurningOn_ValueChanged;
            radioModel.Volume.ValueChanged += Volume_ValueChanged;
            radioModel.VolumePRM.ValueChanged += VolumePRM_ValueChanged;
            radioModel.Sending.ValueChanged += Sending_ValueChanged;
        }

        private bool CheckTurningOn()
        {
            return _radioModel.TurningOn.Value == SwitcherState.Enabled;
        }

        private void UnsubscribeEvents(RadioModel radioModel)
        {
            radioModel.FrequencyNumber.ValueChanged -= FrequencyNumber_ValueChanged;
            radioModel.Interference.ValueChanged -= Interference_ValueChanged;
            radioModel.Noise.ValueChanged -= Noise_ValueChanged;
            radioModel.Power.ValueChanged -= Power_ValueChanged;
            radioModel.RecordWork.ValueChanged -= RecordWork_ValueChanged;
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

            if (model.TurningOn.Value == SwitcherState.Enabled)
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
