﻿using R_173.BL.Tasks;
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
            _message = GetMessageBoxParameters("Begin");
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
            var parameters = GetMessageBoxParameters(ConvertTaskTypeToString(taskType));
            var frequency = TaskHelper.GenerateValidR173Frequency();
            var number = TaskHelper.GenerateValidR173NumpadValue();

            if (taskType == TaskTypes.FrequencyTask)
            {
                parameters.Message = string.Format(parameters.Message, frequency, number);
            }

            parameters.Ok = () =>
            {
                TaskIsRunning = true;
                _radioViewModel.Model.SetInitialState();

                _tasksBl.DataContext
                    .Configure()
                    .SetFrequency(frequency)
                    .SetNumpad(number);

                runningTaskType = taskType;

                _tasksBl.Start(taskType);
            };

            ShowDialog(parameters);
        }

        private static string ConvertTaskTypeToString(TaskTypes taskType)
        {
            switch (taskType)
            {
                case TaskTypes.PreparationToWork:
                    return "Preparation";
                case TaskTypes.PerformanceTest:
                    return "Perfomance";
                case TaskTypes.FrequencyTask:
                    return "WorkingFrequency";
                default:
                    throw new Exception("Unknown TaskType");
            }
        }

        private void ShowDialog(string type)
        {
            var messageBox = App.ServiceCollection.Resolve<IMessageBox>();
            var parameters = GetMessageBoxParameters(type);
            messageBox.ShowDialog(parameters);
        }

        private void ShowDialog(MessageBoxParameters parameters)
        {
            var messageBox = App.ServiceCollection.Resolve<IMessageBox>();
            messageBox.ShowDialog(parameters);
        }

        private void ShowSuccessDialog()
        {
            var messageBox = App.ServiceCollection.Resolve<IMessageBox>();
            var parameters = GetMessageBoxParameters("EndSuccess");
            messageBox.ShowDialog(parameters);
        }

        private void ShowErrorDialog(Message message)
        {
            var messageBox = App.ServiceCollection.Resolve<IMessageBox>();
            var messageBoxParameters = GetMessageBoxParameters("EndFail");
            var item = FormatMessage(message);
            var panel = new StackPanel();
            var text = new TextBlock()
            {
                FontSize = 20,
                TextWrapping = TextWrapping.Wrap,
                Text = messageBoxParameters.Message
            };

            panel.Children.Add(text);
            var tree = new TreeView()
            {
                BorderThickness = new Thickness(0),
                Focusable = false,
                IsManipulationEnabled = false,
            };
            tree.Items.Add(item);

            panel.Children.Add(tree);

            messageBox.InsertBody(messageBoxParameters, panel);
        }

        private void StopTask()
        {
            var message = _tasksBl.Stop();
            _radioViewModel.Model.SetInitialState();
            TaskIsRunning = false;
            _taskViewModels[runningTaskType.Value].NumberOfAttempts++;

            if (message == null)
            {
                ShowDialog("EndSuccess");
                _taskViewModels[runningTaskType.Value].NumberOfSuccessfulAttempts++;
            }
            else
            {
                ShowErrorDialog(message);
            }

            runningTaskType = null;
        }

        private static ControlDescription GetControlDescription(string type)
        {
            var option = App.ServiceCollection.Resolve<ActionDescriptionOption>();

            switch (type)
            {
                case "Begin":
                    return option.Tasks.Begin;
                case "Preparation":
                    return option.Tasks.PreparationToWork;
                case "Perfomance":
                    return option.Tasks.HealthCheck;
                case "WorkingFrequency":
                    return option.Tasks.WorkingFrequencyPreparation;
                case "EndSuccess":
                    return option.Tasks.EndSuccesseful;
                case "EndFail":
                    return option.Tasks.EndFail;
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

            if(type == "Begin" || type == "EndSuccess" || type == "EndFail")
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
