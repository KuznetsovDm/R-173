﻿using R_173.SharedResources;
using R_173.Views.TrainingSteps;
using System.Windows.Input;

namespace R_173.ViewModels
{
    class TrainingViewModel : ViewModelBase
    {
        private int _currentStep;
        private ITrainingStep[] _controls;
        private SimpleCommand _openNextStepCommand;
        private SimpleCommand _openPrevStepCommand;

        public TrainingViewModel()
        {
            _controls = new ITrainingStep[]
            {
                new Preparation(),
                new PerformanceTest(),
                new FrequencyCheck(),
            };
            _openNextStepCommand = new SimpleCommand(() => CurrentStep++);
            _openPrevStepCommand = new SimpleCommand(() => CurrentStep--);

            _currentStep = -1;
            CurrentStep = 0;
        }


        private int CurrentStep
        {
            get => _currentStep;
            set
            {
                if (value == _currentStep || value < 0 || value >= _controls.Length)
                    return;
                _currentStep = value;

                _openNextStepCommand.SetCanExecute = _currentStep < _controls.Length - 1;
                _openPrevStepCommand.SetCanExecute = _currentStep > 0;
                OnPropertyChanged(nameof(CurrentControl));
            }
        }

        public ICommand OpenNextStepCommand => _openNextStepCommand;
        public ICommand OpenPrevStepCommand => _openPrevStepCommand;

        public ITrainingStep CurrentControl => _controls[_currentStep];
    }
}
