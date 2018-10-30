using R_173.Interfaces;
using R_173.Models;
using R_173.SharedResources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace R_173.BL
{
    public delegate bool CheckState(RadioModel model, out IList<string> errors);

    public class Step : IStep<RadioModel>
    {
        protected RadioModel Model;
        protected  CheckState _checkCurrentState;

        public Step(CheckState checkCurrentState)
        {
            _checkCurrentState = checkCurrentState;
        }

        public Step()
        {

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
            errors = Enumerable.Empty<string>().ToList();
            return true;
        }

        public static bool CheckInitialState(RadioModel model, out IList<string> errors)
        {
            errors = new List<string>();

            if (model.Noise.Value != NoiseState.Minimum)
                errors.Add("Noise");

            if (model.Interference.Value != SwitcherState.Disabled)
                errors.Add("Interference");

            if (model.Power.Value != PowerState.Full)
                errors.Add("Power");

            if (model.RecordWork.Value != RecordWorkState.Work)
                errors.Add("Work");

            if (Math.Abs(model.Volume.Value - 0.5) > 0.1)
                errors.Add("Volume");

            if (model.VolumePRM.Value > 0.01)
                errors.Add("VolumePRM");

            return !errors.Any();
        }

        public virtual void Subscribe(RadioModel radioModel)
        {
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

            for (var i = 0; i < radioModel.Numpad.Length; i++)
            {
                radioModel.Numpad[i].ValueChanged += Numpad_ValueChanged;
            }
        }

        public virtual void Unsubscribe(RadioModel radioModel)
        {
            radioModel.Interference.ValueChanged -= Interference_ValueChanged;
            radioModel.Noise.ValueChanged -= Noise_ValueChanged;
            radioModel.Power.ValueChanged -= Power_ValueChanged;
            radioModel.RecordWork.ValueChanged -= RecordWork_ValueChanged;
            radioModel.Tone.ValueChanged -= Tone_ValueChanged;
            radioModel.TurningOn.ValueChanged -= TurningOn_ValueChanged;
            radioModel.Volume.ValueChanged -= Volume_ValueChanged;
            radioModel.VolumePRM.ValueChanged -= VolumePRM_ValueChanged;
            radioModel.FrequencyNumber.ValueChanged -= FrequencyNumber_ValueChanged;

            for (var i = 0; i < radioModel.Numpad.Length; i++)
            {
                radioModel.Numpad[i].ValueChanged -= Numpad_ValueChanged;
            }
        }

        protected virtual void VolumePRM_ValueChanged(object sender, ValueChangedEventArgs<double> e)
        {
        }

        protected virtual void Volume_ValueChanged(object sender, ValueChangedEventArgs<double> e)
        {
        }

        protected virtual void TurningOn_ValueChanged(object sender, ValueChangedEventArgs<SwitcherState> e)
        {
            if (_checkCurrentState == null)
                return;

            if (!_checkCurrentState(Model, out var errors))
            {
                OnStepCrashed(errors);
            }
        }

        protected virtual void Tone_ValueChanged(object sender, ValueChangedEventArgs<SwitcherState> e)
        {
        }

        protected virtual void RecordWork_ValueChanged(object sender, ValueChangedEventArgs<RecordWorkState> e)
        {
        }

        protected virtual void Power_ValueChanged(object sender, ValueChangedEventArgs<PowerState> e)
        {
        }

        protected virtual void Noise_ValueChanged(object sender, ValueChangedEventArgs<NoiseState> e)
        {
        }

        protected virtual void Interference_ValueChanged(object sender, ValueChangedEventArgs<SwitcherState> e)
        {
        }

        protected virtual void FrequencyNumber_ValueChanged(object sender, ValueChangedEventArgs<int> e)
        {
        }

        protected virtual void Numpad_ValueChanged(object sender, ValueChangedEventArgs<SwitcherState> e)
        {
        }

        protected virtual void Board_ValueChanged(object sender, ValueChangedEventArgs<SwitcherState> e)
        {
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
    }
}
