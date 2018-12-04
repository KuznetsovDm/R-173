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
            var taskDescription = _tasksBl.GetDescriptionForTask(taskType, _tasksBl.DataContext);
            ShowDialog(taskDescription);
        }

        private void ShowDialog(string message)
        {
            var messageBox = App.ServiceCollection.Resolve<IMessageBox>();
            messageBox.ShowDialog(() => { }, () => { }, message, "OK", "");
        }

        private void ShowSuccessDialog(string message = "Задача успешно выполнена")
        {
            var messageBox = App.ServiceCollection.Resolve<IMessageBox>();
            messageBox.ShowDialog(() => { }, () => { }, message, "OK", "");
        }

        private void ShowErrorDialog(string message = "Задача не выполнена")
        {
            var messageBox = App.ServiceCollection.Resolve<IMessageBox>();
            messageBox.ShowDialog(() => { }, () => { }, message, "OK", "");
        }

        private void StopTask()
        {
            var message = _tasksBl.Stop();
            _radioViewModel.Model.SetInitialState();
            TaskIsRunning = false;
            if(message == null)
            {
                ShowSuccessDialog();
            }
            else
            {
                //ShowErrorDialog("Задача не выполнена" + Environment.NewLine + message.ToString());

                var item = FormatMessage(message);
                var tree = new TreeView();
                tree.Items.Add(item);

                var messageBox = App.ServiceCollection.Resolve<IMessageBox>();
                messageBox.InsertBody(tree);
            }
        }

        private TreeViewItem FormatMessage(Message message)
        {
            if(string.IsNullOrEmpty(message.Header) && message.Messages.Count() == 1)
            {
                return FormatMessage(message.Messages.First());
            }

            var item = new TreeViewItem()
            {
                IsExpanded = true
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
