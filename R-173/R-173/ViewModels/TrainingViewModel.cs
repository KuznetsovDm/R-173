using R_173.BL.Learning;
using R_173.SharedResources;
using R_173.Views.TrainingSteps;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HPreparation = R_173.Views.TrainingSteps.Horizontal.Preparation;
using HPerformanceTest = R_173.Views.TrainingSteps.Horizontal.PerformanceTest;
using HFrequencyCheck = R_173.Views.TrainingSteps.Horizontal.FrequencyCheck;
using VPreparation = R_173.Views.TrainingSteps.Vertical.Preparation;
using VPerformanceTest = R_173.Views.TrainingSteps.Vertical.PerformanceTest;
using VFrequencyCheck = R_173.Views.TrainingSteps.Vertical.FrequencyCheck;

namespace R_173.ViewModels
{
    class TrainingViewModel : ViewModelBase
    {
        private readonly ITrainingStep[] _horizontalControls;
        private readonly ITrainingStep[] _verticalControls;
        private readonly SimpleCommand _openNextStepCommand;
        private readonly SimpleCommand _openPrevStepCommand;
        private readonly SimpleCommand _startOverCommand;
        private readonly SimpleCommand _changeOrientationCommand;
        private readonly RadioViewModel _radioViewModel;
        private readonly LearningBL _learning;
        private int _maxStep;
        private int _currentStep;
        private Orientation _orientation;

        public TrainingViewModel()
        {
            var viewModels = new[]
            {
                new TrainingStepViewModel(),
                new TrainingStepViewModel(),
                new TrainingStepViewModel(),
            };
            _horizontalControls = new ITrainingStep[]
            {
                new HPreparation() { DataContext = viewModels[0] },
                new HPerformanceTest() { DataContext = viewModels[1] },
                new HFrequencyCheck() { DataContext = viewModels[2] },
            };
            _verticalControls = new ITrainingStep[]
            {
                new VPreparation() { DataContext = viewModels[0] },
                new VPerformanceTest() { DataContext = viewModels[1] },
                new VFrequencyCheck() { DataContext = viewModels[2] },
            };

            _openNextStepCommand = new SimpleCommand(() => CurrentStep++, () => _currentStep < _horizontalControls.Length && _currentStep < _maxStep);
            _openPrevStepCommand = new SimpleCommand(() => CurrentStep--, () => _currentStep > 1);
            _changeOrientationCommand = new SimpleCommand(() => Orientation =
                Orientation == Orientation.Horizontal
                ? Orientation.Vertical
                : Orientation.Horizontal);
            _radioViewModel = new RadioViewModel();
            _learning = new LearningBL(_radioViewModel.Model, Learning_Completed, Learning_StepChanged, _horizontalControls[0].Type);
            _startOverCommand = new SimpleCommand(StartOver);
            _maxStep = 1;
            CurrentStep = 1;
            Orientation = Orientation.Horizontal;
        }

        public int CurrentStep
        {
            get => _currentStep;
            set
            {
                if (value == _currentStep || value < 1 || value > _horizontalControls.Length/* || value > _maxStep*/)
                    return;
                _currentStep = value;

                _openNextStepCommand.OnCanExecuteChanged();
                _openPrevStepCommand.OnCanExecuteChanged();
                OnPropertyChanged(nameof(CurrentHorizontalControl));
                OnPropertyChanged(nameof(CurrentVerticalControl));
                OnPropertyChanged(nameof(Caption));
                OnPropertyChanged(nameof(CurrentStep));
                _learning.SetCurrentLearning(CurrentHorizontalControl.Type);
            }
        }

        public ICommand OpenNextStepCommand => _openNextStepCommand;
        public ICommand OpenPrevStepCommand => _openPrevStepCommand;
        public ICommand StartOverCommand => _startOverCommand;
        public ICommand ChangeOrientationCommand => _changeOrientationCommand;

        public ITrainingStep CurrentHorizontalControl => _horizontalControls[_currentStep - 1];
        public ITrainingStep CurrentVerticalControl => _verticalControls[_currentStep - 1];

        public string Caption => _horizontalControls[_currentStep - 1].Caption;
        public RadioViewModel RadioViewModel => _radioViewModel;
        public int StepsNumber => _horizontalControls.Length;

        public Orientation Orientation
        {
            get => _orientation;
            set
            {
                if (value == _orientation)
                    return;
                _orientation = value;
                OnPropertyChanged(nameof(Orientation));
            }
        }


        private void Learning_StepChanged(int step)
        {
            var viewModel = (TrainingStepViewModel)_horizontalControls[_currentStep - 1].DataContext;
            viewModel.CurrentStep = step;
        }

        private void Learning_Completed()
        {
            _maxStep = Math.Max(_maxStep, _currentStep + 1);
            _openNextStepCommand.OnCanExecuteChanged();
            CurrentStep++;
            MessageBox.Show("Completed");
        }

        private void StartOver()
        {
            CurrentStep = 1;
            _learning.Restart();
        }
    }
}
