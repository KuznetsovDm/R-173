using R_173.BL.Tasks;
using R_173.SharedResources;
using System;
using System.Collections;
using System.Windows;
using System.Windows.Input;
using HPreparation = R_173.Views.TrainingSteps.Horizontal.Preparation;
using HPerformanceTest = R_173.Views.TrainingSteps.Horizontal.PerformanceTest;
using HFrequencyCheck = R_173.Views.TrainingSteps.Horizontal.FrequencyCheck;
using R_173.BE;

namespace R_173.ViewModels
{
    class TasksViewModel : ViewModelBase
    {
        private readonly TaskViewModel[] _tasks;
        private readonly SimpleCommand _stopTaskCommand;
        private bool _taskIsRunning;
        private RadioViewModel _radioViewModel;
        private TasksBl _tasksBl;

        public RadioViewModel RadioViewModel => _radioViewModel;

        public TasksViewModel()
        {
            _tasks = new[]
            {
                new TaskViewModel(HPreparation.StepCaption, () => StartTask(TaskTypes.PreparationToWork)),
                new TaskViewModel(HPerformanceTest.StepCaption, () => StartTask(TaskTypes.PerformanceTest)),
                new TaskViewModel(HFrequencyCheck.StepCaption, () => StartTask(TaskTypes.FrequencyTask)),
            };
            _stopTaskCommand = new SimpleCommand(StopTask);
            _radioViewModel = new RadioViewModel();
            _tasksBl = new TasksBl(_radioViewModel.Model);
        }

        public IEnumerable Tasks => _tasks;
        public ICommand StopTaskCommand => _stopTaskCommand;

        public bool TaskIsRunning
        {
            get => _taskIsRunning;
            set
            {
                if (value == _taskIsRunning)
                    return;
                _taskIsRunning = value;
                OnPropertyChanged(nameof(TaskIsRunning));
            }
        }

        private void StartTask(TaskTypes taskType)
        {
            TaskIsRunning = true;
            _radioViewModel.Model.SetInitialState();
            _tasksBl.DataContext
                .Configure()
                .SetFrequency(TaskHelper.GenerateValidR173Frequency())
                .SetNumpad(TaskHelper.GenerateValidR173NumpadValue());

            _tasksBl.Start(taskType);
            MessageBox.Show(_tasksBl.GetDescriptionForTask(taskType, _tasksBl.DataContext));
        }

        private void StopTask()
        {
            var errors = _tasksBl.Stop();
            _radioViewModel.Model.SetInitialState();
            TaskIsRunning = false;
            MessageBox.Show(string.Join(Environment.NewLine, errors));
        }
    }
}
