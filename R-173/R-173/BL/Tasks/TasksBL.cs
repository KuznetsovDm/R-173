using R_173.BL.Learning;
using R_173.Handlers;
using R_173.Models;
using System.Collections.Generic;

namespace R_173.BL.Tasks
{
    public class TasksBl
    {
        private RadioModel _model;
        private List<Task> _tasks;

        public TasksBl(RadioModel model, KeyboardHandler handler = null)
        {
            var learningFactory = new LearningFactory(handler);

            _model = model;

            _tasks.Add(new Task(model, learningFactory.CreatePreparationToWorkLearning()));
            _tasks.Add(new Task(model, learningFactory.CreatePerformanceTestLearning()));
            _tasks.Add(new Task(model, learningFactory.CreateSettingFrequencies()));
        }


    }
}
