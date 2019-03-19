using System;
using R_173.BE;
using R_173.BL.Learning;
using R_173.Models;

namespace R_173.BL.Tasks
{
    public class TasksBl
    {
        private readonly TaskFactory _taskFactory;
	    private Task _activeTask;

        public TasksBl(RadioModel model, TaskDataContext dataContext = null)
        {
            DataContext = dataContext ?? new TaskDataContext();
            var learningFactory = new LearningFactory();
            _taskFactory = new TaskFactory(model, learningFactory);
        }

        public TaskDataContext DataContext { get; }

        public void Start(TaskTypes taskType)
        {
            switch (taskType)
            {
                case TaskTypes.PreparationToWork: _activeTask = _taskFactory.CreatePreparationToWorkTask(); break;
                case TaskTypes.PerformanceTest: _activeTask = _taskFactory.CreatePerfomanceTestTask(); break;
                case TaskTypes.FrequencyTask:
                    _activeTask = _taskFactory.CreateFrequencyTask(DataContext.NumpadNumber, DataContext.Frequency); break;
	            case TaskTypes.ConnectionEasy:
		            break;
	            case TaskTypes.ConnectionHard:
		            break;
	            default: throw new Exception($"Invalid state {taskType}.");
            }
            _activeTask.Start();
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
        public int ComputerNumber { get; private set; }

		public DataContextBuilder Configure()
        {
            return new DataContextBuilder(this);
        }

        public class DataContextBuilder
        {
            private readonly TaskDataContext _taskDataContext;

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

	        public DataContextBuilder SetComputerNumber(int value)
	        {
		        _taskDataContext.ComputerNumber = value;
		        return this;
	        }
		}
    }
}
