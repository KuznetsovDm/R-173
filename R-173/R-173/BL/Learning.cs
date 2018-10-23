using R_173.Models;
using R_173.SharedResources;
using System;
using System.Collections.Generic;

namespace R_173.BL
{
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
}
