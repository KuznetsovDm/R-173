using R_173.BE;
using R_173.BL.Learning;
using R_173.Models;
using System;
using System.Collections.Generic;
using System.Windows;

namespace R_173.BL.Tasks
{
    public class TasksBl
    {
        private readonly TaskFactory _taskFactory;
        private readonly RadioModel _model;
        private Task _activeTask = null;

        public TasksBl(RadioModel model, TaskDataContext dataContext = null)
        {
            DataContext = dataContext ?? new TaskDataContext();
            var learningFactory = new LearningFactory();
            _taskFactory = new TaskFactory(model, learningFactory);
            _model = model;
        }

        public TaskDataContext DataContext { get; }

        public void Start(TaskTypes taskType)
        {
            int numpad = TaskHelper.GenerateValidR173NumpadValue();
            int frequency = TaskHelper.GenerateValidR173Frequency();

            switch (taskType)
            {
                case TaskTypes.PreparationToWork: _activeTask = _taskFactory.CreatePreparationToWorkTask(); break;
                case TaskTypes.PerformanceTest: _activeTask = _taskFactory.CreatePerfomanceTestTask(); break;
                case TaskTypes.FrequencyTask:
                    _activeTask = _taskFactory.CreateFrequencyTask(DataContext.NumpadNumber, DataContext.Frequency); break;
                default: throw new Exception($"Invalid state {taskType}.");
            }
            _activeTask.Start();
        }

        public string GetDescriptionForTask(TaskTypes taskType, TaskDataContext context)
        {
            switch (taskType)
            {
                case TaskTypes.PreparationToWork: return "Подготовьте радиостанцию к работе.";
                case TaskTypes.PerformanceTest: return "Проверьте работоспособность радиостанции.";
                case TaskTypes.FrequencyTask: return $"Фиксированную частоту {context.NumpadNumber} настройте на частоту {context.Frequency}.";
                default: throw new ArgumentException($"Unknown TaskType {taskType}.");
            }
        }

        public Message Stop()
        {
            return _activeTask.Stop();
        }
    }

    public class TaskDataContext
    {
        public int NumpadNumber { get; private set; }
        public int Frequency { get; private set; }

        public DataContextBuilder Configure()
        {
            return new DataContextBuilder(this);
        }

        public class DataContextBuilder
        {
            private TaskDataContext _taskDataContext;

            public DataContextBuilder(TaskDataContext taskDataContext)
            {
                _taskDataContext = taskDataContext;
            }

            public DataContextBuilder SetNumpad(int value)
            {
                _taskDataContext.NumpadNumber = value;
                return this;
            }

            public DataContextBuilder SetFrequency(int value)
            {
                _taskDataContext.Frequency = value;
                return this;
            }
        }
    }
}
