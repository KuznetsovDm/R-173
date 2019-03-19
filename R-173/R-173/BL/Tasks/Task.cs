using R_173.BE;
using R_173.BL.Learning;
using R_173.Models;
using System;

namespace R_173.BL.Tasks
{
    public class Task
    {
        private readonly RadioModel _model;
        private readonly CompositeStep _step;
        private bool _taskCompleted;

	    public Task(RadioModel model, CompositeStep step)
        {
            _model = model;
            _step = step;
        }

        public void Start()
        {
			_model.SetRandomState();
	        if (!_step.StartIfInputConditionsAreRight(_model, out var _)) return;

	        _taskCompleted = false;
	        _step.StepChanged += Step_StepChanged;
	        _step.Completed += Step_Completed;
        }

        private void Step_Completed(object sender, EventArgs e)
        {
            _taskCompleted = true;
        }

        private static void Step_StepChanged(object sender, SharedResources.StepChangedEventArgs e)
        {
        }

        public Message Stop()
        {
            Message message = null;
            if (!_taskCompleted)
            {
                message = new Message { Header = null, Messages = new[] { _step.GetErrorDescription() } };
            }
	        _model.SetRandomState();
			_step.Reset();
            return message;
        }
    }

    public static class TaskHelper
    {
        private static readonly Random Rand = new Random();
	    private const int MinR173Frequency = 30000;
	    private const int MaxR173Frequency = 75999;
	    private const int MaxR173NumpadNumber = 9;

	    public static int GenerateValidR173Frequency()
        {
            return GeneratNumberInRange(MinR173Frequency, MaxR173Frequency + 1);
        }

        public static int GenerateValidR173NumpadValue()
        {
            return GeneratNumberInRange(0, MaxR173NumpadNumber + 1);
        }

	    public static int GenerateComputerNumber()
	    {
		    return GeneratNumberInRange(1, 21);
	    }

		private static int GeneratNumberInRange(int min, int max)
        {
            return Rand.Next(min, max);
        }
    }
}
