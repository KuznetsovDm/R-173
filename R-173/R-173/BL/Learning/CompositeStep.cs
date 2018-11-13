using R_173.Models;
using R_173.Interfaces;
using R_173.SharedResources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace R_173.BL.Learning
{
    public class CompositeStep : IStep<RadioModel>, IDisposable
    {
        IList<IStep<RadioModel>> _steps;
        private int _current = 0;
        private RadioModel _model;

        public event EventHandler<StepChangedEventArgs> StepChanged = (e, args) => { };

        public CompositeStep(IList<IStep<RadioModel>> steps)
        {
            _steps = steps;
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

            //если дошли, то значит шаг запустился
            _steps[_current].Completed += Step_Completed;
            _steps[_current].Crashed += Step_Crashed;

            StepChanged(this, new StepChangedEventArgs { Step = _current });
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

                StepChanged(this, new StepChangedEventArgs { Step = _current });
            }
            else
            {
                throw new InvalidOperationException($"WTF!!! current={_current}" +
                    $"{Environment.NewLine}" +
                    $"{{{string.Join(",", errors)}}}");
            }
        }

        public event EventHandler Completed = (e, args) => { };
        public event EventHandler<CrashedEventArgs> Crashed = (e, args) => { };

        public bool StartIfInputConditionsAreRight(RadioModel model, out IList<string> errors)
        {
            _model = model;
            if (_steps[_current].StartIfInputConditionsAreRight(model, out errors))
            {
                _steps[_current].Completed += Step_Completed;
                _steps[_current].Crashed += Step_Crashed;
            }

            return !errors.Any();
        }

        public void Freeze()
        {
            if (_current >= 0 && _current < _steps.Count)
            {
                _steps[_current].Freeze();
            }
        }

        public void Unfreeze()
        {
            if (_current >= 0 && _current < _steps.Count)
            {
                _steps[_current].Unfreeze();
            }
        }

        public void Reset()
        {
            if (_current >= 0 && _current < _steps.Count)
            {
                _steps[_current].Reset();
                _steps[_current].Completed -= Step_Completed;
                _steps[_current].Crashed -= Step_Crashed;
            }
            _current = 0;
        }

        public void Dispose()
        {

        }
    }
}
