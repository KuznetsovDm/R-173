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
using R_173.Interfaces;
using Unity;
using System.Windows.Controls;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Media;

namespace R_173.ViewModels
{
    class TasksViewModel : ViewModelBase, ITabWithMessage
    {
        private readonly MessageBoxParameters _message;
        private readonly TaskViewModel[] _tasks;
        private readonly SimpleCommand _stopTaskCommand;
        private bool _taskIsRunning;
        private RadioViewModel _radioViewModel;
        private TasksBl _tasksBl;
        private TaskTypes? runningTaskType = null;
        private Dictionary<TaskTypes, TaskViewModel> _taskViewModels;

        public RadioViewModel RadioViewModel => _radioViewModel;

        public TasksViewModel()
        {
            _message = new MessageBoxParameters("Tab tasks", "message");
            _taskViewModels = new Dictionary<TaskTypes, TaskViewModel>
            {
                { TaskTypes.PreparationToWork, new TaskViewModel(HPreparation.StepCaption, () => StartTask(TaskTypes.PreparationToWork))},
                { TaskTypes.PerformanceTest, new TaskViewModel(HPerformanceTest.StepCaption, () => StartTask(TaskTypes.PerformanceTest))},
                { TaskTypes.FrequencyTask, new TaskViewModel(HFrequencyCheck.StepCaption, () => StartTask(TaskTypes.FrequencyTask))},
            };
            _tasks = new[]
            {
                _taskViewModels[TaskTypes.PreparationToWork],
                _taskViewModels[TaskTypes.PerformanceTest],
                _taskViewModels[TaskTypes.FrequencyTask],
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

        public MessageBoxParameters Message => _message;

        private void StartTask(TaskTypes taskType)
        {
            TaskIsRunning = true;
            _radioViewModel.Model.SetInitialState();
            _tasksBl.DataContext
                .Configure()
                .SetFrequency(TaskHelper.GenerateValidR173Frequency())
                .SetNumpad(TaskHelper.GenerateValidR173NumpadValue());

            runningTaskType = taskType;

            _tasksBl.Start(taskType);
            var taskDescription = _tasksBl.GetDescriptionForTask(taskType, _tasksBl.DataContext);
            ShowDialog("Начало задачи", taskDescription);
        }

        private void ShowDialog(string title, string message)
        {
            var messageBox = App.ServiceCollection.Resolve<IMessageBox>();
            messageBox.ShowDialog(title, message);
        }

        private void ShowSuccessDialog(string title = "Задача успешно выполнена", string message = "")
        {
            var messageBox = App.ServiceCollection.Resolve<IMessageBox>();
            messageBox.ShowDialog(title, message);
        }

        private void ShowErrorDialog(Message message, string title = "Задача не выполнена")
        {
            var messageBox = App.ServiceCollection.Resolve<IMessageBox>();
            var item = FormatMessage(message);
            var tree = new TreeView()
            {
                BorderThickness = new Thickness(0),
                Focusable = false,
                IsManipulationEnabled = false,
            };
            tree.Items.Add(item);

            messageBox.InsertBody(title, tree);
        }

        private void StopTask()
        {
            var message = _tasksBl.Stop();
            _radioViewModel.Model.SetInitialState();
            TaskIsRunning = false;
            _taskViewModels[runningTaskType.Value].NumberOfAttempts++;
            if (message == null)
            {
                ShowSuccessDialog();
                _taskViewModels[runningTaskType.Value].NumberOfSuccessfulAttempts++;
            }
            else
            {
                ShowErrorDialog(message, "Задача не выполнена");
            }
            runningTaskType = null;
        }

        private TreeViewItem FormatMessage(Message message)
        {
            if(string.IsNullOrEmpty(message.Header) && message.Messages.Count() == 1)
            {
                return FormatMessage(message.Messages.First());
            }

            var item = new TreeViewItem()
            {
                IsExpanded = true,
                Foreground = new SolidColorBrush(Colors.Red),
                FontSize = 18
            };

            if (!string.IsNullOrEmpty(message.Header))
            {
                item.Header = message.Header;
            }

            if (message.Messages == null)
            {
                return item;
            }

            foreach (var m in message.Messages)
            {
                item.Items.Add(FormatMessage(m));
            }

            return item;
        }
    }
}
