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

            _tasks.Add(taskFactory.CreatePerfomanceTestTask());
        }

        public void Start()
        {
            _tasks[0].Start();
        }

        public IEnumerable<string> Stop()
        {
            return _tasks[0].Stop();
        }

    }
}
