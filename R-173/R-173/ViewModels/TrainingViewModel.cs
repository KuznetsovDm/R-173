using R_173.BL;
using R_173.SharedResources;
using R_173.Views.TrainingSteps;
using System.Windows.Input;

namespace R_173.ViewModels
{
    class TrainingViewModel : ViewModelBase
    {
        private readonly ITrainingStep[] _controls;
        private readonly TrainingStepViewModel[] _trainingStepViewModels;
        private readonly SimpleCommand _openNextStepCommand;
        private readonly SimpleCommand _openPrevStepCommand;
        private readonly RadioViewModel _radioViewModel;
        private readonly Learning _learning;
        private int _currentLearningStep;
        private int _currentStep;

        public TrainingViewModel()
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
            _radioViewModel = new RadioViewModel();
            _currentStep = -1;
            CurrentStep = 0;
            _learning = new Learning(_radioViewModel.Model, Learning_Completed, Learning_StepChanged);
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
                OnPropertyChanged(nameof(Caption));
            }
        }

        public ICommand OpenNextStepCommand => _openNextStepCommand;
        public ICommand OpenPrevStepCommand => _openPrevStepCommand;

        public ITrainingStep CurrentControl => _controls[_currentStep];

        public string Caption => _controls[_currentStep].Caption;
        public RadioViewModel RadioViewModel => _radioViewModel;


        private void Learning_StepChanged(int step)
        {
            _trainingStepViewModels[_currentStep].CurrentStep = step;
        }

        private void Learning_Completed()
        {

        }
    }
}
