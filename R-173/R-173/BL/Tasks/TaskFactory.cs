using R_173.BL.Learning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace R_173.BL.Tasks
{
    public class TaskFactory
    {
        private LearningFactory _learningFactory;

        public TaskFactory(LearningFactory learningFactory)
        {
            _learningFactory = learningFactory;
        }

        public CompositeStep CreatePerformanceTestStep()
        {
            return new CompositeStepBuilder()
                .Add(_learningFactory.CreatePreparationToWorkLearning(1))
                .Add(new VolumeChangeStep(
                    checkInputConditions: PerformanceTestLearning.CheckWorkingState,
                    checkInternalState: PerformanceTestLearning.CheckWorkingState))
                .Add(new NoiseChangedStep(
                    checkInputConditions: PerformanceTestLearning.CheckWorkingState,
                    checkInternalState: PerformanceTestLearning.CheckWorkingState))
                .Add(new PrdPressStep(
                    checkInputConditions: PerformanceTestLearning.CheckWorkingState,
                    checkInternalState: PerformanceTestLearning.CheckWorkingState))
                .Add(new PressToneStep(
                    checkInputConditions: PerformanceTestLearning.CheckWorkingState,
                    checkInternalState: PerformanceTestLearning.CheckWorkingState))
                .Build();
        }
    }
}
