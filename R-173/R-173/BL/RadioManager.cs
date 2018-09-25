using P2PMulticastNetwork.Interfaces;
using P2PMulticastNetwork.Model;
using R_173.Interfaces;
using R_173.Models;
using R_173.SharedResources;

namespace R_173.BL
{
    public class RadioManager : IRadioManager
    {
        private RadioModel _radioModel;
        private IAudioReaderAndSender<SendableRadioModel> _reader;
        private IAudioReceiverAndPlayer<ReceivableRadioModel> _player;

        public RadioManager(IAudioReaderAndSender<SendableRadioModel> reader,
            IAudioReceiverAndPlayer<ReceivableRadioModel> player)
        {
            _reader = reader;
            _player = player;
        }

        public void SetModel(RadioModel radioModel)
        {
            if(_radioModel != null)
            {
                _radioModel.Frequency.ValueChanged -= Frequency_ValueChanged;
                _radioModel.Interference.ValueChanged -= Interference_ValueChanged;
                _radioModel.LeftPuOa.ValueChanged -= LeftPuOa_ValueChanged;
                _radioModel.Noise.ValueChanged -= Noise_ValueChanged;
                _radioModel.Power.ValueChanged -= Power_ValueChanged;
                _radioModel.RecordWork.ValueChanged -= RecordWork_ValueChanged;
                _radioModel.RightPuOa.ValueChanged -= RightPuOa_ValueChanged;
                _radioModel.Tone.ValueChanged -= Tone_ValueChanged;
                _radioModel.TurningOn.ValueChanged -= TurningOn_ValueChanged;
                _radioModel.Volume.ValueChanged -= Volume_ValueChanged;
                _radioModel.VolumePRM.ValueChanged -= VolumePRM_ValueChanged;
            }

            _radioModel = radioModel;

            #region Events
            _radioModel.Frequency.ValueChanged += Frequency_ValueChanged;
            _radioModel.Interference.ValueChanged += Interference_ValueChanged;
            _radioModel.LeftPuOa.ValueChanged += LeftPuOa_ValueChanged;
            _radioModel.Noise.ValueChanged += Noise_ValueChanged;
            _radioModel.Power.ValueChanged += Power_ValueChanged;
            _radioModel.RecordWork.ValueChanged += RecordWork_ValueChanged;
            _radioModel.RightPuOa.ValueChanged += RightPuOa_ValueChanged;
            _radioModel.Tone.ValueChanged += Tone_ValueChanged;
            _radioModel.TurningOn.ValueChanged += TurningOn_ValueChanged;
            _radioModel.Volume.ValueChanged += Volume_ValueChanged;
            _radioModel.VolumePRM.ValueChanged += VolumePRM_ValueChanged;
            #endregion
        }

        #region Events
        private void VolumePRM_ValueChanged(object sender, ValueChangedEventArgs<int> e)
        {

        }

        private void Volume_ValueChanged(object sender, ValueChangedEventArgs<int> e)
        {
            _player.SetModel(GetReceivableRadioModelFromRadioModel(_radioModel));
        }

        private void TurningOn_ValueChanged(object sender, ValueChangedEventArgs<SwitcherState> e)
        {
            if (e.NewValue == SwitcherState.Enabled)
            {
                _reader.Start();
            }
            else
            {
                _reader.Stop();
                _player.Stop();
            }
        }

        private void Tone_ValueChanged(object sender, ValueChangedEventArgs<SwitcherState> e)
        {

        }

        private void RightPuOa_ValueChanged(object sender, ValueChangedEventArgs<SwitcherState> e)
        {

        }

        private void RecordWork_ValueChanged(object sender, ValueChangedEventArgs<RecordWorkState> e)
        {
            if(e.NewValue == RecordWorkState.Record) // запись
            {
                _reader.Start();
                _player.Stop();
            }
            else
            {
                _reader.Stop();
                _player.Start();
            }
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

        private void Frequency_ValueChanged(object sender, ValueChangedEventArgs<int> e)
        {
            _reader.SetModel(GetSendableRadioModelFromRadioModel(_radioModel));
            _player.SetModel(GetReceivableRadioModelFromRadioModel(_radioModel));
        }
        #endregion

        private static SendableRadioModel GetSendableRadioModelFromRadioModel(RadioModel radioModel)
        {
            return new SendableRadioModel
            {
                Frequency = radioModel.Frequency.Value
            };
        }

        private static ReceivableRadioModel GetReceivableRadioModelFromRadioModel(RadioModel radioModel)
        {
            return new ReceivableRadioModel
            {
                Frequency = radioModel.Frequency.Value,
                Noise = radioModel.Noise.Value == NoiseState.Minimum, // TODO: noise
                Volume = radioModel.Volume.Value
            };
        }
    }
}
