using R_173.BE;
using R_173.BL.Learning;
using R_173.Models;
using System.Collections.Generic;

namespace R_173.BL.Tasks
{
    public class TasksBl
    {
        private readonly RadioModel _model;
        private List<Task> _tasks = new List<Task>();
        private int _activeTask = 0;

        public TasksBl(RadioModel model)
        {
            var learningFactory = new LearningFactory();
            var taskFactory = new TaskFactory(model, learningFactory);
            _model = model;

            _tasks.Add(taskFactory.CreatePreparationToWorkTask());
            _tasks.Add(taskFactory.CreatePerfomanceTestTask());
        }

        public void Start(TaskTypes taskType)
        {
            switch (taskType)
            {
                case TaskTypes.PreparationToWork: _activeTask = 0; break;
                case TaskTypes.PerformanceTest: _activeTask = 1; break;
                case TaskTypes.FrequencyTask: _activeTask = 2; break;
                default: throw new System.Exception($"Invalid state {taskType}.");
            }
            _tasks[_activeTask].Start();
        }

        public Message Stop()
        {
            return _tasks[_activeTask].Stop();
        }

    }
}
