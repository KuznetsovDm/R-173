using R_173.Models;
using R_173.SharedResources;
using R_173.Views.TrainingSteps;
using System;
using System.Collections.Generic;

namespace R_173.BL.Learning
{
    public class LearningBl
    {
        private readonly Action _completed;
        private readonly Action<int> _stepChanged;
        private readonly List<CompositeStep> _learnings = new List<CompositeStep>();
        private int _currentLearning;
        private readonly RadioModel _model;

        public LearningBl(RadioModel model, Action completed, Action<int> stepChanged, StepsTypes learningType)
        {
            var learningFactory = new LearningFactory();
            _completed = completed;
            _stepChanged = stepChanged;
            _model = model;

            _learnings.Add(learningFactory.CreatePreparationToWorkLearning());
            _learnings.Add(learningFactory.CreatePerformanceTestLearning());
            _learnings.Add(learningFactory.CreateSettingFrequencies());

            InitAll();

            SetCurrentLearning(learningType);
        }

        private void Completed(object sender, EventArgs args)
        {
            _completed();
        }

        private void StepChanged(object sender, StepChangedEventArgs args)
        {
            _stepChanged(args.Step);
        }

        public void SetCurrentLearning(StepsTypes learningType)
        {
            switch (learningType)
            {
	            case StepsTypes.Preparation:
		            _currentLearning = 0;
		            break;
	            case StepsTypes.PerformanceTest:
		            _currentLearning = 1;
		            break;
	            case StepsTypes.FrequencyCheck:
		            _currentLearning = 2;
					break;
	            default:
		            _currentLearning = 0;
		            break;
            }

            FreezeAll();
            _learnings[_currentLearning].Unfreeze();
        } 

        public void FreezeAll()
        {
            foreach (var learning in _learnings)
            {
                learning.Freeze();
            }
        }

        public void InitAll()
        {
            foreach (var learning in _learnings)
            {
                learning.StartIfInputConditionsAreRight(_model, out var _);
                learning.Completed += Completed;
                learning.StepChanged += StepChanged;
            }
        }

        public void Restart()
        {
            ResetAll();
            FreezeAll();
            _learnings[_currentLearning].Unfreeze();
        }

        private void ResetAll()
        {
            foreach (var learning in _learnings)
            {
                learning.Reset();
                learning.StartIfInputConditionsAreRight(_model, out var _);
			}
        }
    }
}
