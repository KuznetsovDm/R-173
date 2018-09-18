using P2PMulticastNetwork.Interfaces;
using R_173.Models;
using R_173.SharedResources;

namespace R_173.BL
{
    public class RadioManager
    {
        private RadioModel _radioModel;
        private IAudioReaderAndSender<SendableRadioModel> _sender;
        private IAudioReceiverAndPlayer<ReceivableRadioModel> _receiver;

        public RadioManager(RadioModel radioModel, IAudioReaderAndSender<SendableRadioModel> sender,
            IAudioReceiverAndPlayer<ReceivableRadioModel> receiver)
        {
            _radioModel = radioModel;
            _sender = sender;
            _receiver = receiver;

            _sender.Start();
            _receiver.Start();

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
        }

        private void VolumePRM_ValueChanged(object sender, ValueChangedEventArgs<int> e)
        {
            throw new System.NotImplementedException();
        }

        private void Volume_ValueChanged(object sender, ValueChangedEventArgs<int> e)
        {
            throw new System.NotImplementedException();
        }

        private void TurningOn_ValueChanged(object sender, ValueChangedEventArgs<SwitcherState> e)
        {
            throw new System.NotImplementedException();
        }

        private void Tone_ValueChanged(object sender, ValueChangedEventArgs<SwitcherState> e)
        {
            throw new System.NotImplementedException();
        }

        private void RightPuOa_ValueChanged(object sender, ValueChangedEventArgs<SwitcherState> e)
        {
            throw new System.NotImplementedException();
        }

        private void RecordWork_ValueChanged(object sender, ValueChangedEventArgs<SwitcherState> e)
        {
            throw new System.NotImplementedException();
        }

        private void Power_ValueChanged(object sender, ValueChangedEventArgs<SwitcherState> e)
        {
            throw new System.NotImplementedException();
        }

        private void Noise_ValueChanged(object sender, ValueChangedEventArgs<SwitcherState> e)
        {
            throw new System.NotImplementedException();
        }

        private void LeftPuOa_ValueChanged(object sender, ValueChangedEventArgs<SwitcherState> e)
        {
            throw new System.NotImplementedException();
        }

        private void Interference_ValueChanged(object sender, ValueChangedEventArgs<SwitcherState> e)
        {
            throw new System.NotImplementedException();
        }

        private void Frequency_ValueChanged(object sender, ValueChangedEventArgs<int> e)
        {
            _sender.SetFilter(GetSendableRadioModelFromRadioModel(_radioModel));
            _receiver.SetFilter(GetReceivableRadioModelFromRadioModel(_radioModel));
        }

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
                Noise = radioModel.Noise.Value == SwitcherState.Enabled,
                Volume = radioModel.Volume.Value
            };
        }
    }
}
