using R_173.BL.Learning;
using R_173.SharedResources;
using R_173.Views.TrainingSteps;
using System;
using System.Windows;
using System.Windows.Input;

namespace R_173.ViewModels
{
    class TrainingViewModel : ViewModelBase
    {
        private readonly ITrainingStep[] _controls;
        private readonly SimpleCommand _openNextStepCommand;
        private readonly SimpleCommand _openPrevStepCommand;
        private readonly SimpleCommand _startOverCommand;
        private readonly RadioViewModel _radioViewModel;
        private readonly LearningBL _learning;
        private int _maxStep;
        private int _currentStep;

        public TrainingViewModel()
        {
            _controls = new ITrainingStep[]
            {
                new Preparation() { DataContext = new TrainingStepViewModel() },
                new PerformanceTest() { DataContext = new TrainingStepViewModel() },
                new FrequencyCheck() { DataContext = new TrainingStepViewModel() },
            };
            _openNextStepCommand = new SimpleCommand(() => CurrentStep++, () => _currentStep < _controls.Length && _currentStep < _maxStep);
            _openPrevStepCommand = new SimpleCommand(() => CurrentStep--, () => _currentStep > 1);
            _radioViewModel = new RadioViewModel();
            _learning = new LearningBL(_radioViewModel.Model, Learning_Completed, Learning_StepChanged, typeof(Preparation));
            _startOverCommand = new SimpleCommand(() => _learning.Restart());
            _maxStep = 1;
            CurrentStep = 1;
        }

        public int CurrentStep
        {
            get => _currentStep;
            private set
            {
                if (value == _currentStep || value < 1 || value > _controls.Length || value > _maxStep)
                    return;
                _currentStep = value;

                _openNextStepCommand.OnCanExecuteChanged();
                _openPrevStepCommand.OnCanExecuteChanged();
                OnPropertyChanged(nameof(CurrentControl));
                OnPropertyChanged(nameof(Caption));
                OnPropertyChanged(nameof(CurrentStep));
                _learning.SetCurrentLearning(_controls[_currentStep - 1].GetType());
            }
        }

        public ICommand OpenNextStepCommand => _openNextStepCommand;
        public ICommand OpenPrevStepCommand => _openPrevStepCommand;
        public ICommand StartOverCommand => _startOverCommand;

        public ITrainingStep CurrentControl => _controls[_currentStep - 1];

        public string Caption => _controls[_currentStep - 1].Caption;
        public RadioViewModel RadioViewModel { get; }
        public int StepsNumber => _controls.Length;


        private void Learning_StepChanged(int step)
        {
            var viewModel = (TrainingStepViewModel)_controls[_currentStep - 1].DataContext;
            viewModel.CurrentStep = step;
        }

        private void Learning_Completed()
        {
            _maxStep = Math.Max(_maxStep, _currentStep + 1);
            _openNextStepCommand.OnCanExecuteChanged();
            MessageBox.Show("Completed");
        }
    }
}
