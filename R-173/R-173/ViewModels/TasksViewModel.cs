using R_173.SharedResources;
using System;
using System.Collections;
using System.Windows;
using System.Windows.Input;
using HPreparation = R_173.Views.TrainingSteps.Horizontal.Preparation;
using HPerformanceTest = R_173.Views.TrainingSteps.Horizontal.PerformanceTest;
using HFrequencyCheck = R_173.Views.TrainingSteps.Horizontal.FrequencyCheck;

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
                new TaskViewModel(HPreparation.StepCaption, () => StartTask(typeof(HPreparation))),
                new TaskViewModel(HPerformanceTest.StepCaption, () => StartTask(typeof(HPerformanceTest))),
                new TaskViewModel(HFrequencyCheck.StepCaption, () => StartTask(typeof(HFrequencyCheck))),
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
