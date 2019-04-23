using System;
using P2PMulticastNetwork.Model;
using R_173.BL.Learning;
using R_173.Interfaces;
using R_173.Models;

namespace R_173.BL.Tasks
{
    public class TaskFactory
    {
        private readonly LearningFactory _learningFactory;
        private readonly RadioModel _model;
	    private readonly INetworkTaskManager _networkTaskManager;
	    private readonly INetworkTaskListener _networkTaskListener;

        public TaskFactory(RadioModel radioModel,
	        LearningFactory learningFactory,
	        INetworkTaskManager networkTaskManager,
	        INetworkTaskListener networkTaskListener)
        {
	        _model = radioModel;
			_learningFactory = learningFactory;
	        _networkTaskManager = networkTaskManager;
	        _networkTaskListener = networkTaskListener;
        }

        public ITask CreatePerfomanceTestTask()
        {
            return new Task(_model, CreatePerformanceTestStep());
        }

        public ITask CreatePreparationToWorkTask()
        {
            return new Task(_model, _learningFactory.CreatePreparationToWorkLearning(1));
        }

        public ITask CreateFrequencyTask(int notepadNumber, int frequency)
        {
            var builder = new CompositeStepBuilder();
            var step = builder.Add(_learningFactory.CreatePreparationToWorkLearning(1, 3))
                    .Add(new RecordWorkToRecordStep(
                    checkInputConditions: PerformanceTestLearning.CheckWorkingState))
                    .Add(new FrequencySetStep(notepadNumber, frequency))
                    .Add(new RecordWorkToWorkStep())
                    .Build();

            return new Task(_model, step);
        }

	    public ITask CreateConnectionEasyTask(NetworkTaskData data)
	    {
		    return new NetworkTask(_networkTaskManager, data, _networkTaskListener);
	    }

	    public ITask CreateConnectionHardTask(int notepadNumber, int frequency, int computerNumber)
	    {
		    throw new NotImplementedException();
	    }

		private CompositeStep CreatePerformanceTestStep()
        {
            return new CompositeStepBuilder()
                .Add(_learningFactory.CreatePreparationToWorkLearning(1, 3))
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
