using R_173.BL.Learning;
using R_173.Handlers;
using R_173.SharedResources;
using R_173.Views.TrainingSteps;
using System.Windows;
using System.Windows.Input;

namespace R_173.ViewModels
{
    class TrainingViewModel : ViewModelBase
    {
        private readonly ITrainingStep[] _controls;
        private readonly TrainingStepViewModel[] _trainingStepViewModels;
        private readonly SimpleCommand _openNextStepCommand;
        private readonly SimpleCommand _openPrevStepCommand;
        private readonly LearningBL _learning;
        private int _currentLearningStep;
        private int _currentStep;

        public TrainingViewModel(KeyboardHandler keyboardHandler)
        {
            _trainingStepViewModels = new TrainingStepViewModel[]
            {
                new TrainingStepViewModel(),
                new TrainingStepViewModel(),
                new TrainingStepViewModel(),
            };
            _controls = new ITrainingStep[]
            {
                new Preparation() { DataContext = _trainingStepViewModels[0] },
                new PerformanceTest() { DataContext = _trainingStepViewModels[1] },
                new FrequencyCheck() { DataContext = _trainingStepViewModels[2] },
            };
            _openNextStepCommand = new SimpleCommand(() => CurrentStep++);
            _openPrevStepCommand = new SimpleCommand(() => CurrentStep--);
            RadioViewModel = new RadioViewModel();
            _learning = new LearningBL(keyboardHandler, RadioViewModel.Model, Learning_Completed, Learning_StepChanged, typeof(Preparation));
            CurrentStep = 1;
        }

        public int CurrentStep
        {
            get => _currentStep;
            private set
            {
                if (value == _currentStep || value < 1 || value > _controls.Length)
                    return;
                _currentStep = value;

                _openNextStepCommand.SetCanExecute = _currentStep < _controls.Length;
                _openPrevStepCommand.SetCanExecute = _currentStep > 1;
                OnPropertyChanged(nameof(CurrentControl));
                OnPropertyChanged(nameof(Caption));
                OnPropertyChanged(nameof(CurrentStep));
                _learning.SetCurrentLearning(_controls[_currentStep - 1].GetType());
            }
        }

        public ICommand OpenNextStepCommand => _openNextStepCommand;
        public ICommand OpenPrevStepCommand => _openPrevStepCommand;

        public ITrainingStep CurrentControl => _controls[_currentStep - 1];

        public string Caption => _controls[_currentStep - 1].Caption;
        public RadioViewModel RadioViewModel { get; }
        public int StepsNumber => _controls.Length;


        private void Learning_StepChanged(int step)
        {
            _trainingStepViewModels[_currentStep - 1].CurrentStep = step;
        }

        private void Learning_Completed()
        {
            MessageBox.Show("Completed");
        }
    }
}
