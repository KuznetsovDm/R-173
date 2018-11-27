using R_173.BE;
using R_173.BL.Learning;
using R_173.Handlers;
using R_173.Models;
using System.Collections.Generic;

namespace R_173.BL.Tasks
{
    public class TasksBl
    {
        private readonly RadioModel _model;
        private List<Task> _tasks = new List<Task>();

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
                case TaskTypes.PreparationToWork: _tasks[0].Start(); break;
                case TaskTypes.PerformanceTest: _tasks[1].Start(); break;
                case TaskTypes.FrequencyTask: _tasks[2].Start(); break;
                default: throw new System.Exception($"Invalid state {taskType}."); break;
            }
        }

        public IEnumerable<string> Stop()
        {
            return _tasks[0].Stop();
        }

    }
}
