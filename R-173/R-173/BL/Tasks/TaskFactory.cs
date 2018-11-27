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

        public Task CreateFrequencyTask(int notepadNumber, int frequency)
        {
            var builder = new CompositeStepBuilder();
            var step = builder.Add(_learningFactory.CreatePreparationToWorkLearning(1, 3))
                    .Add(new RecordWorkToRecordStep(
                    checkInputConditions: PerformanceTestLearning.CheckWorkingState))
                    .Add(new FrequencySetStep(notepadNumber, frequency,
                    checkInputConditions: SettingFrequenciesLearning.CheckRecordState,
                    checkInternalState: SettingFrequenciesLearning.CheckRecordState))
                    .Add(new RecordWorkToWorkStep())
                    .Build();

            return new Task(_model, step);
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
