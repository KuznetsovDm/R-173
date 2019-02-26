using R_173.BL.Learning;
using R_173.SharedResources;
using R_173.Views.TrainingSteps;
using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using HPreparation = R_173.Views.TrainingSteps.Horizontal.Preparation;
using HPerformanceTest = R_173.Views.TrainingSteps.Horizontal.PerformanceTest;
using HFrequencyCheck = R_173.Views.TrainingSteps.Horizontal.FrequencyCheck;
using VPreparation = R_173.Views.TrainingSteps.Vertical.Preparation;
using VPerformanceTest = R_173.Views.TrainingSteps.Vertical.PerformanceTest;
using VFrequencyCheck = R_173.Views.TrainingSteps.Vertical.FrequencyCheck;
using R_173.Interfaces;
using Unity;
using System.Windows;
using MahApps.Metro.Controls.Dialogs;
using R_173.BE;

namespace R_173.ViewModels
{
    public class TrainingViewModel : ViewModelBase, ITabWithMessage
    {
        private readonly ITrainingStep[] _horizontalControls;
        private readonly ITrainingStep[] _verticalControls;
        private readonly MessageBoxParameters _message;
        private readonly SimpleCommand _openNextStepCommand;
        private readonly SimpleCommand _openPrevStepCommand;
        private readonly SimpleCommand _startOverCommand;
        private readonly SimpleCommand _changeOrientationCommand;
        private readonly RadioViewModel _radioViewModel;
        private readonly LearningBL _learning;
        private int _maxStep;
        private int _currentStep;
        private bool _popupIsOpen;
        private FrameworkElement _blockUnderMouse;
        private Orientation _orientation;
        private readonly TrainingStepViewModel[] _viewModels;
        private object _currentToolTip;
        private bool _crutch = true;

        public TrainingViewModel()
        {
            _viewModels = new[]
            {
                new TrainingStepViewModel(5),
                new TrainingStepViewModel(8),
                new TrainingStepViewModel(9),
            };
            _horizontalControls = new ITrainingStep[]
            {
                new HPreparation() { DataContext = _viewModels[0] },
                new HPerformanceTest() { DataContext = _viewModels[1] },
                new HFrequencyCheck() { DataContext = _viewModels[2] },
            };
            _verticalControls = new ITrainingStep[]
            {
                new VPreparation() { DataContext = _viewModels[0] },
                new VPerformanceTest() { DataContext = _viewModels[1] },
                new VFrequencyCheck() { DataContext = _viewModels[2] },
            };
            _message = GetMessageBoxParameters("Preparation.Begin");

            _openNextStepCommand = new SimpleCommand(() => CurrentStep++, () => _currentStep < _horizontalControls.Length && _currentStep < _maxStep);
            _openPrevStepCommand = new SimpleCommand(() => CurrentStep--, () => _currentStep > 1);
            _changeOrientationCommand = new SimpleCommand(() => Orientation =
                Orientation == Orientation.Horizontal
                ? Orientation.Vertical
                : Orientation.Horizontal);
            _radioViewModel = new RadioViewModel();
            _radioViewModel.Model.SetInitialState();
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
                if (value == _currentStep || value < 1 || value > _horizontalControls.Length)
                {
                    _viewModels[_currentStep - 1].CurrentStep = 0;
                    return;
                }
                _currentStep = value;

                _openNextStepCommand.OnCanExecuteChanged();
                _openPrevStepCommand.OnCanExecuteChanged();
                OnPropertyChanged(nameof(CurrentHorizontalControl));
                OnPropertyChanged(nameof(CurrentVerticalControl));
                OnPropertyChanged(nameof(Caption));
                OnPropertyChanged(nameof(CurrentStep));
                _learning.SetCurrentLearning(CurrentHorizontalControl.Type);
                _viewModels[_currentStep - 1].CurrentStep = 0;

                if (_crutch)
                {
                    _crutch = false;
                    return;
                }

                var type = ConvertStepNumberToString(_currentStep) + ".Begin";
                var parameters = GetMessageBoxParameters(type);
                ShowDialog(parameters);
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

        public bool PopupIsOpen
        {
            get => _popupIsOpen;
            set
            {
                if (value == _popupIsOpen)
                    return;
                _popupIsOpen = value;
                OnPropertyChanged(nameof(PopupIsOpen));
            }
        }

        public FrameworkElement BlockUnderMouse
        {
            get => _blockUnderMouse;
            set
            {
                if (value == _blockUnderMouse)
                    return;
                _blockUnderMouse = value;
                OnPropertyChanged(nameof(BlockUnderMouse));
            }
        }

        public object CurrentToolTip
        {
            get => _currentToolTip;
            set
            {
                if (value == _currentToolTip)
                    return;
                _currentToolTip = value;
                OnPropertyChanged(nameof(CurrentToolTip));
            }
        }
        
        public MessageBoxParameters Message => _message;

        private void Learning_StepChanged(int step)
        {
            _viewModels[_currentStep - 1].CurrentStep = step;
        }

        private void Learning_Completed()
        {
            _maxStep = Math.Max(_maxStep, _currentStep + 1);

            _viewModels[_currentStep - 1].SetMaxStep();

            var type = ConvertStepNumberToString(CurrentStep) + ".End";
            var parameters = GetMessageBoxParameters(type);
            parameters.Cancel = StartOver;
            parameters.Ok = GetMessageBoxOkAction(CurrentStep);
            ShowDialog(parameters);
        }
        
        private static string ConvertStepNumberToString(int stepNumber)
        {
            switch (stepNumber)
            {
                case 1:
                    return "Preparation";
                case 2:
                    return "Performance";
                case 3:
                    return "WorkingFrequency";
                default:
                    throw new Exception("Unknown step number.");
            }
        }

        private void ShowDialog(MessageBoxParameters parameters)
        {
            var messageBox = App.ServiceCollection.Resolve<IMessageBox>();
            messageBox.ShowDialog(parameters);

            //var mainWindow = App.ServiceCollection.Resolve<MainWindow>();
            //mainWindow.ShowOverlay();
            //await mainWindow.ShowMessageAsync(parameters.Title, parameters.Message, MessageDialogStyle.AffirmativeAndNegative,
            //    new MetroDialogSettings()
            //    {
            //        AffirmativeButtonText = parameters.OkText,
            //        ColorScheme = MetroDialogColorScheme.Theme,
            //        AnimateShow = true,
            //        NegativeButtonText = parameters.CancelText,

            //    }).ContinueWith(task =>
            //{
            //    var result = task.Result;
            //    if (result == MessageDialogResult.Negative)
            //        parameters.Cancel();
            //    else parameters.Ok();
            //});
        }

        private Action GetMessageBoxOkAction(int stepNumber)
        {
            if (stepNumber == 3)
            {
                var mainWindow = App.ServiceCollection.Resolve<MainWindow>();
                return mainWindow.GoToTaskTab;
            }

            return () =>
            {
                _openNextStepCommand.OnCanExecuteChanged();
                CurrentStep++;
            };
        }

        private void StartOver()
        {
            _radioViewModel.Model.SetInitialState();
            CurrentStep = 1;
            _learning.Restart();
        }



        private static ControlDescription GetControlDescription(string type)
        {
            var option = App.ServiceCollection.Resolve<ActionDescriptionOption>();

            switch (type)
            {
                case "Preparation.Begin":
                    return option.PreparationToWork.Begin;
                case "Preparation.End":
                    return option.PreparationToWork.End;
                case "Performance.Begin":
                    return option.HealthCheck.Begin;
                case "Performance.End":
                    return option.HealthCheck.End;
                case "WorkingFrequency.Begin":
                    return option.WorkingFrequencyPreparation.Begin;
                case "WorkingFrequency.End":
                    return option.WorkingFrequencyPreparation.End;
                default:
                    throw new Exception("Unknown message box type.");
            }
        }

        private static MessageBoxParameters GetMessageBoxParameters(string type)
        {
            var description = GetControlDescription(type);

            var parameters = new MessageBoxParameters
            {
                Title = description.Title,
                Message = description.Body
            };

            if (type.EndsWith("Begin"))
            {
                parameters.OkText = description.Buttons[0];
            }
            else
            {
                parameters.CancelText = description.Buttons[0];
                parameters.OkText = description.Buttons[1];
            }

            return parameters;
        }

    }
}
