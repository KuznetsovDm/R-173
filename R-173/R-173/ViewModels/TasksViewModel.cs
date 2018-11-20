using R_173.BL.Tasks;
using R_173.SharedResources;
using R_173.Views.TrainingSteps;
using System;
using System.Collections;
using System.Windows;
using System.Windows.Input;

namespace R_173.ViewModels
{
    class TasksViewModel : ViewModelBase
    {
        private readonly TaskViewModel[] _tasks;
        private readonly SimpleCommand _stopTaskCommand;
        private bool _taskIsRunning;

        public TasksViewModel()
        {
            _tasks = new[]
            {
                new TaskViewModel(Preparation.StepCaption, () => StartTask(typeof(Preparation))),
                new TaskViewModel(PerformanceTest.StepCaption, () => StartTask(typeof(PerformanceTest))),
                new TaskViewModel(FrequencyCheck.StepCaption, () => StartTask(typeof(FrequencyCheck))),
            };
            _stopTaskCommand = new SimpleCommand(StopTask);

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


        private void StartTask(Type taskType)
        {
            TaskIsRunning = true;

            MessageBox.Show(taskType.ToString());
        }

        private void StopTask()
        {
            TaskIsRunning = false;
        }
    }
}
