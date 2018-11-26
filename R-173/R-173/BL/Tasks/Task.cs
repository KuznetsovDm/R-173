using R_173.BL.Learning;
using R_173.Models;
using System;
using System.Collections.Generic;

namespace R_173.BL.Tasks
{
    public class Task
    {
        private readonly RadioModel _model;
        private CompositeStep _step;
        private bool _taskStarted = false;
        private bool _taskCompleted = false;
        private int _currentStep = 0;

        public Task(RadioModel model, CompositeStep step)
        {
            _model = model;
            _step = step;
        }

        public void Start()
        {
            if(_step.StartIfInputConditionsAreRight(_model, out var errors))
            {
                _taskStarted = true;
                _taskCompleted = false;
                _step.StepChanged += _step_StepChanged;
                _step.Completed += _step_Completed;
            }
        }

        private void _step_Completed(object sender, EventArgs e)
        {
            _taskCompleted = true;
        }

        private void _step_StepChanged(object sender, SharedResources.StepChangedEventArgs e)
        {
            _currentStep = e.Step;
        }

        public IEnumerable<string> Stop()
        {
            var errors = new List<string>();
            if (!_taskCompleted)
            {
                errors.Add("Задача не выполнена.");
                errors.Add("Текущий шаг: " + _currentStep);
                errors.Add(_step.GetErrorDescription());
            }
            _step.Reset();
            return errors;
        }
    }
}
