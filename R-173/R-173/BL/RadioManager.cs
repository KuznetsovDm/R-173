﻿using P2PMulticastNetwork.Interfaces;
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

            if (_radioModel != null)
            {
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
            else
            {
            }
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
            if (e.NewValue == RecordWorkState.Record) // запись
            {
                _reader.StartListenMicrophone();
                _player.Stop();
            }
            else
            {
                _reader.StopListenMicrophone();
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
    }
}
