using R_173.Interfaces;
using R_173.Models;
using R_173.SharedResources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace R_173.BL.Learning
{
    public delegate bool CheckState(RadioModel model, out IList<string> errors);

    public class Step : IStep<RadioModel>
    {
        protected RadioModel Model;
        protected CheckState _checkInputConditions;
        protected CheckState _checkInternalState;
        private bool _isSubscribed = false;

        public Step(CheckState checkInputConditions = null, CheckState checkInternalState = null)
        {
            _checkInputConditions = checkInputConditions;
            _checkInternalState = checkInternalState;
        }

        public event EventHandler Completed = (e, args) => { };
        public event EventHandler<CrashedEventArgs> Crashed = (e, args) => { };

        public bool StartIfInputConditionsAreRight(RadioModel model, out IList<string> errors)
        {
            if (!CheckInputConditions(model, out errors))
            {
                return false;
            }

            Model = model;
            Subscribe(model);
            return true;
        }

        public virtual bool CheckInputConditions(RadioModel model, out IList<string> errors)
        {
            if (_checkInputConditions != null)
            {
                return _checkInputConditions(model, out errors);
            }

            errors = Enumerable.Empty<string>().ToList();
            return true;
        }

        public virtual void Subscribe(RadioModel radioModel)
        {
            if (radioModel == null || _isSubscribed)
                return;

            radioModel.Interference.ValueChanged += Interference_ValueChanged;
            radioModel.Noise.ValueChanged += Noise_ValueChanged;
            radioModel.Power.ValueChanged += Power_ValueChanged;
            radioModel.RecordWork.ValueChanged += RecordWork_ValueChanged;
            radioModel.Tone.ValueChanged += Tone_ValueChanged;
            radioModel.TurningOn.ValueChanged += TurningOn_ValueChanged;
            radioModel.Volume.ValueChanged += Volume_ValueChanged;
            radioModel.VolumePRM.ValueChanged += VolumePRM_ValueChanged;
            radioModel.FrequencyNumber.ValueChanged += FrequencyNumber_ValueChanged;
            radioModel.Board.ValueChanged += Board_ValueChanged;
            radioModel.Sending.ValueChanged += Sending_ValueChanged;
            radioModel.Reset.ValueChanged += Reset_ValueChanged;

            for (var i = 0; i < radioModel.Numpad.Length; i++)
            {
                radioModel.Numpad[i].ValueChanged += Numpad_ValueChanged;
            }

            _isSubscribed = true;
        }

        public virtual void Unsubscribe(RadioModel radioModel)
        {
            if (radioModel == null || !_isSubscribed)
                return;

            radioModel.Interference.ValueChanged -= Interference_ValueChanged;
            radioModel.Noise.ValueChanged -= Noise_ValueChanged;
            radioModel.Power.ValueChanged -= Power_ValueChanged;
            radioModel.RecordWork.ValueChanged -= RecordWork_ValueChanged;
            radioModel.Tone.ValueChanged -= Tone_ValueChanged;
            radioModel.TurningOn.ValueChanged -= TurningOn_ValueChanged;
            radioModel.Volume.ValueChanged -= Volume_ValueChanged;
            radioModel.VolumePRM.ValueChanged -= VolumePRM_ValueChanged;
            radioModel.FrequencyNumber.ValueChanged -= FrequencyNumber_ValueChanged;
            radioModel.Board.ValueChanged -= Board_ValueChanged;
            radioModel.Sending.ValueChanged -= Sending_ValueChanged;
            radioModel.Reset.ValueChanged -= Reset_ValueChanged;

            for (var i = 0; i < radioModel.Numpad.Length; i++)
            {
                radioModel.Numpad[i].ValueChanged -= Numpad_ValueChanged;
            }

            _isSubscribed = false;
        }

        protected virtual void Reset_ValueChanged(object sender, ValueChangedEventArgs<SwitcherState> e)
        {
            SomethingChanged();
        }

        protected virtual void Sending_ValueChanged(object sender, ValueChangedEventArgs<SwitcherState> e)
        {
            SomethingChanged();
        }

        protected virtual void VolumePRM_ValueChanged(object sender, ValueChangedEventArgs<double> e)
        {
            SomethingChanged();
        }

        protected virtual void Volume_ValueChanged(object sender, ValueChangedEventArgs<double> e)
        {
            SomethingChanged();
        }

        protected virtual void TurningOn_ValueChanged(object sender, ValueChangedEventArgs<SwitcherState> e)
        {
            SomethingChanged();
        }

        protected virtual void Tone_ValueChanged(object sender, ValueChangedEventArgs<SwitcherState> e)
        {
            SomethingChanged();
        }

        protected virtual void RecordWork_ValueChanged(object sender, ValueChangedEventArgs<RecordWorkState> e)
        {
            SomethingChanged();
        }

        protected virtual void Power_ValueChanged(object sender, ValueChangedEventArgs<PowerState> e)
        {
            SomethingChanged();
        }

        protected virtual void Noise_ValueChanged(object sender, ValueChangedEventArgs<NoiseState> e)
        {
            SomethingChanged();
        }

        protected virtual void Interference_ValueChanged(object sender, ValueChangedEventArgs<SwitcherState> e)
        {
            SomethingChanged();
        }

        protected virtual void FrequencyNumber_ValueChanged(object sender, ValueChangedEventArgs<int> e)
        {
            SomethingChanged();
        }

        protected virtual void Numpad_ValueChanged(object sender, ValueChangedEventArgs<SwitcherState> e)
        {
            SomethingChanged();
        }

        protected virtual void Board_ValueChanged(object sender, ValueChangedEventArgs<SwitcherState> e)
        {
            SomethingChanged();
        }

        protected virtual void SomethingChanged()
        {
            if (_checkInternalState == null)
                return;

            if (!_checkInternalState(Model, out var errors))
            {
                OnStepCrashed(errors);
            }
        }

        protected void OnStepCompleted()
        {
            Unsubscribe(Model);
            Completed(this, EventArgs.Empty);
        }

        protected void OnStepCrashed(IList<string> errors)
        {
            Unsubscribe(Model);
            Crashed(this, new CrashedEventArgs { Errors = errors });
        }

        public void Freeze()
        {
            Unsubscribe(Model);
        }

        public void Unfreeze()
        {
            Subscribe(Model);
        }

        public void Reset()
        {
            Unsubscribe(Model);
        }

        public void Dispose()
        {
        }
    }
}
