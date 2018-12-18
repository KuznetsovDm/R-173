using R_173.BE;
using R_173.BL.Learning;
using R_173.Models;
using System;

namespace R_173.BL.Tasks
{
    public class Task
    {
        private readonly RadioModel _model;
        private CompositeStep _step;
        private bool _taskCompleted = false;
        private int _currentStep = 0;

        public Task(RadioModel model, CompositeStep step)
        {
            _model = model;
            _step = step;
        }

        public void Start()
        {
            if (_step.StartIfInputConditionsAreRight(_model, out var errors))
            {
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

        public Message Stop()
        {
            Message message = null;
            if (!_taskCompleted)
            {
                message = new Message { Header = null, Messages = new[] { _step.GetErrorDescription() } };
            }
            _step.Reset();
            return message;
        }
    }

    public static class TaskHelper
    {
        private static Random _rand = new Random();
        private static int _minR173Frequency = 30000;
        private static int _maxR173Frequency = 75999;
        private static int _maxR173NumpadNumber = 9;

        public static int GenerateValidR173Frequency()
        {
            return GeneratNumberInRange(_minR173Frequency, _maxR173Frequency + 1);
        }

        public static int GenerateValidR173NumpadValue()
        {
            return GeneratNumberInRange(0, _maxR173NumpadNumber + 1);
        }

        public static int GeneratNumberInRange(int min, int max)
        {
            return _rand.Next(min, max);
        }
    }
}
