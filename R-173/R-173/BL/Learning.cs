using R_173.Models;
using R_173.SharedResources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace R_173.BL
{
    public class StepChangedEventArgs : EventArgs
    {
        public int Step { get; set; }
    }

    public class CrashedEventArgs : EventArgs
    {
        public IList<string> Errors { get; set; }
    }

    public class Learning
    {
        private List<Step> _steps;
        private int _currentStep = 0;
        private RadioModel _model;

        public event EventHandler<StepChangedEventArgs> StepChanged;

        public Learning(List<Step> steps)
        {
            _steps = steps;
        }

        public void Start(RadioModel model)
        {
            _model = model;
        }

        private void OnStepChanged(int newStepNumber)
        {
            var step = _steps[newStepNumber];
            if (step.StartIfInputConditionsAreRight(_model, out IList<string> errors))
            {
                step.Completed += StepCompleted;
                step.Crashed += StepCrashed;
            }
            else
            {
                System.Diagnostics.Trace.WriteLine("Errors:");

                foreach (var error in errors)
                {
                    System.Diagnostics.Trace.WriteLine(error);
                }
            }
        }

        private void StepCrashed(object sender, EventArgs e)
        {
            _currentStep--;
            var step = _steps[_currentStep];

            step.Completed -= StepCompleted;
            step.Crashed -= StepCrashed;

            OnStepChanged(_currentStep);
        }

        private void StepCompleted(object sender, EventArgs e)
        {
            _currentStep++;
            var step = _steps[_currentStep];

            step.Completed -= StepCompleted;
            step.Crashed -= StepCrashed;

            OnStepChanged(_currentStep);
        }

        public void Stop()
        {

        }

        public static IList<Predicate<RadioModel>> GetWorkingFrequencyPreparationChecks()
        {
            IList<Predicate<RadioModel>> result = new List<Predicate<RadioModel>>
            {
                // перейти на запись
                model =>
                {
                    return model.RecordWork.Value == RecordWorkState.Record;
                },

                // выбрать номер частоты и нажать СБРОС
                model =>
                {
                    return model.Reset.Value == SwitcherState.Enabled;
                },

                // последовательно нажать 5 цифр


                // перейти на работу
                model =>
                {
                    return model.RecordWork.Value == RecordWorkState.Work;
                },

            };

            return result;
        }
    }

    public enum StepState
    {
        NotStarted,
        InProcess,
        Completed
    }

    public class CompositeStep : IStep<RadioModel>
    {
        IList<IStep<RadioModel>> _steps;
        private int _current = 0;
        private RadioModel _model;

        public CompositeStep(IList<IStep<RadioModel>> steps)
        {
            _steps = steps;
        }

        private void OnStepChange(int prevStateNumber, int current)
        {

        }

        private void Step_Crashed(object sender, CrashedEventArgs e)
        {
            var prevStateNumber = _current;
            _steps[prevStateNumber].Completed -= Step_Completed;
            _steps[prevStateNumber].Crashed -= Step_Crashed;

            if (_current == 0)
            {
                Crashed(this, e);
                return;
            }

            _current--;

            IList<string> errors = null;
            while (_current >= 0 && !_steps[_current].StartIfInputConditionsAreRight(_model, out errors))
            {
                _current--;
            }

            if (_current == -1)
            {
                Crashed(this, new CrashedEventArgs { Errors = errors });
                return;
            }

            //если дошли, то значит шаг запустился.
            _steps[_current].Completed += Step_Completed;
            _steps[_current].Crashed += Step_Crashed;
        }

        private void Step_Completed(object sender, EventArgs e)
        {
            var prevStateNumber = _current;
            _steps[prevStateNumber].Completed -= Step_Completed;
            _steps[prevStateNumber].Crashed -= Step_Crashed;

            bool isCurrentEndOfSteps = _current == _steps.Count - 1;
            if (isCurrentEndOfSteps)
            {
                Completed(this, EventArgs.Empty);
                return;
            }

            _current++;

            if (_steps[_current].StartIfInputConditionsAreRight(_model, out var errors))
            {
                _steps[_current].Completed += Step_Completed;
                _steps[_current].Crashed += Step_Crashed;
            }
            else
            {
                throw new InvalidOperationException($"WTF!!! current={_current}" +
                    $"{Environment.NewLine}" +
                    $"{{{String.Join(",", errors)}}}");
            }
        }

        public event EventHandler Completed;
        public event EventHandler<CrashedEventArgs> Crashed;

        public bool StartIfInputConditionsAreRight(RadioModel model, out IList<string> errors)
        {
            _model = model;
            _steps[_current].Completed += Step_Completed;
            _steps[_current].Crashed += Step_Crashed;
            return _steps[0].StartIfInputConditionsAreRight(model, out errors);
        }
    }

    public class CompositeStepBuilder
    {
        private List<IStep<RadioModel>> _steps = new List<IStep<RadioModel>>();

        public CompositeStepBuilder Add(IStep<RadioModel> step)
        {
            _steps.Add(step);
            return this;
        }

        public CompositeStep Build()
        {
            return new CompositeStep(_steps);
        }
    }

    public class Step : IStep<RadioModel>
    {
        protected RadioModel Model;

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
        }

        protected virtual void VolumePRM_ValueChanged(object sender, ValueChangedEventArgs<double> e)
        {
        }

        protected virtual void Volume_ValueChanged(object sender, ValueChangedEventArgs<double> e)
        {
        }

        protected virtual void TurningOn_ValueChanged(object sender, ValueChangedEventArgs<SwitcherState> e)
        {
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

    public class TurningOnStep : Step
    {
        protected override void TurningOn_ValueChanged(object sender, ValueChangedEventArgs<SwitcherState> e)
        {
            System.Diagnostics.Trace.WriteLine("Turning on: " + e.NewValue);
            OnStepCompleted();
        }
    }
}
