using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using R_173.BE;
using R_173.BL.Tasks;
using R_173.Helpers;
using R_173.Interfaces;
using R_173.SharedResources;
using Unity;
using HPreparation = R_173.Views.TrainingSteps.Horizontal.Preparation;
using HPerformanceTest = R_173.Views.TrainingSteps.Horizontal.PerformanceTest;
using HFrequencyCheck = R_173.Views.TrainingSteps.Horizontal.FrequencyCheck;

namespace R_173.ViewModels
{
    class TasksViewModel : ViewModelBase, ITabWithMessage
    {
	    private readonly TaskViewModel[] _tasks;
        private readonly SimpleCommand _stopTaskCommand;
        private bool _taskIsRunning;
	    private readonly TasksBl _tasksBl;
        private TaskTypes? _runningTaskType;
        private readonly Dictionary<TaskTypes, TaskViewModel> _taskViewModels;

        public RadioViewModel RadioViewModel { get; }

	    public TasksViewModel()
        {
            Message = GetMessageBoxParameters("Begin");

	        var option = App.ServiceCollection.Resolve<ActionDescriptionOption>();

			_taskViewModels = new Dictionary<TaskTypes, TaskViewModel>
            {
                { TaskTypes.PreparationToWork, new TaskViewModel(option.Tasks.PreparationToWork.Title, () => StartTask(TaskTypes.PreparationToWork))},
                { TaskTypes.PerformanceTest, new TaskViewModel(option.Tasks.HealthCheck.Title, () => StartTask(TaskTypes.PerformanceTest))},
                { TaskTypes.FrequencyTask, new TaskViewModel(option.Tasks.WorkingFrequencyPreparation.Title, () => StartTask(TaskTypes.FrequencyTask))},
                { TaskTypes.ConnectionEasy, new TaskViewModel(option.Tasks.ConnectionEasy.Title, () => StartTask(TaskTypes.ConnectionEasy))},
                { TaskTypes.ConnectionHard, new TaskViewModel(option.Tasks.ConnectionHard.Title, () => StartTask(TaskTypes.ConnectionHard))},
            };
            _tasks = new[]
            {
                _taskViewModels[TaskTypes.PreparationToWork],
                _taskViewModels[TaskTypes.PerformanceTest],
                _taskViewModels[TaskTypes.FrequencyTask],
                _taskViewModels[TaskTypes.ConnectionEasy],
                _taskViewModels[TaskTypes.ConnectionHard],
			};
            _stopTaskCommand = new SimpleCommand(StopTask);
            RadioViewModel = new RadioViewModel();
            _tasksBl = new TasksBl(RadioViewModel.Model);
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

	    public int Assessment => _tasks.Sum(t => t.NumberOfSuccessfulAttempts > 0 ? 1 : 0);

	    public MessageBoxParameters Message { get; }

	    private void StartTask(TaskTypes taskType)
        {
            var parameters = GetMessageBoxParameters(ConvertTaskTypeToString(taskType));

            var frequency = TaskHelper.GenerateValidR173Frequency();
            var number = TaskHelper.GenerateValidR173NumpadValue();
            var computerNumber = TaskHelper.GenerateComputerNumber();

			if (taskType == TaskTypes.FrequencyTask ||
			    taskType == TaskTypes.ConnectionEasy ||
			    taskType == TaskTypes.ConnectionHard)
            {
                parameters.Message = string.Format(parameters.Message, frequency, number, computerNumber);
            }

            parameters.Ok = () =>
            {
                TaskIsRunning = true;
                RadioViewModel.Model.SetRandomState();

                _tasksBl.DataContext
                    .Configure()
                    .SetFrequency(frequency)
                    .SetNumpad(number)
		            .SetComputerNumber(computerNumber);

                _runningTaskType = taskType;

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
	            case TaskTypes.ConnectionEasy:
		            return "ConnectionEasy";
	            case TaskTypes.ConnectionHard:
		            return "ConnectionHard";
				default:
                    throw new Exception("Unknown TaskType");
            }
        }

        private static void ShowDialog(string type)
        {
            var parameters = GetMessageBoxParameters(type);
            ShowDialog(parameters);
        }

        private static void ShowDialog(MessageBoxParameters parameters)
        {
            System.Threading.Tasks.Task.Factory.StartNew(async () => await MetroMessageBoxHelper.ShowDialog(parameters),
                CancellationToken.None,
                TaskCreationOptions.None,
                TaskScheduler.FromCurrentSynchronizationContext());
        }

        private static void ShowErrorDialog(Message message)
        {
            var messageBoxParameters = GetMessageBoxParameters("EndFail");
            var item = FormatMessage(message);
            var panel = new StackPanel();
            var text = new TextBlock
            {
                FontSize = 20,
                TextWrapping = TextWrapping.Wrap,
                Text = messageBoxParameters.Message
            };

            panel.Children.Add(text);
            var tree = new TreeView
            {
                BorderThickness = new Thickness(0),
                Focusable = false,
                IsManipulationEnabled = false
            };
            tree.Items.Add(item);

            panel.Children.Add(tree);

            var messageBox = App.ServiceCollection.Resolve<IMessageBox>();
            messageBox.InsertBody(messageBoxParameters, panel);
        }

        private void StopTask()
        {
            var message = _tasksBl.Stop();
            TaskIsRunning = false;

            if (_runningTaskType != null)
            {
                var taskType = _runningTaskType.Value;

                _taskViewModels[taskType].NumberOfAttempts++;

                if (message == null)
                {
                    ShowDialog("EndSuccess");
                    _taskViewModels[taskType].NumberOfSuccessfulAttempts++;
                }
                else
                {
                    ShowErrorDialog(message);
                }
            }
			OnPropertyChanged(nameof(Assessment));
            _runningTaskType = null;
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
	            case "ConnectionEasy":
		            return option.Tasks.ConnectionEasy;
	            case "ConnectionHard":
		            return option.Tasks.ConnectionHard;
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

            if (type == "Begin" || type == "EndSuccess" || type == "EndFail")
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

        private static TreeViewItem FormatMessage(Message message)
        {
            if (string.IsNullOrEmpty(message.Header) && message.Messages.Count() == 1)
            {
                return FormatMessage(message.Messages.First());
            }

            var item = new TreeViewItem
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
