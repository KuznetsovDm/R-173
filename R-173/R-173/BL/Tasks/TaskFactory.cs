using R_173.BL.Learning;
using R_173.Models;

namespace R_173.BL.Tasks
{
    public class TaskFactory
    {
        private LearningFactory _learningFactory;
        private RadioModel _model;

        public TaskFactory(RadioModel radioModel, LearningFactory learningFactory)
        {
            _learningFactory = learningFactory;
            _model = radioModel;
        }

        public Task CreatePerfomanceTestTask()
        {
            return new Task(_model, CreatePerformanceTestStep());
        }

        public Task CreatePreparationToWorkTask()
        {
            return new Task(_model, _learningFactory.CreatePreparationToWorkLearning(1));
        }

        private CompositeStep CreatePerformanceTestStep()
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
