using R_173.Models;
using R_173.SharedResources;
using R_173.Views.TrainingSteps;
using System;
using System.Collections.Generic;

namespace R_173.BL
{
    public class Learning
    {
        private readonly Action _completed;
        private readonly Action<int> _stepChanged;
        private readonly List<CompositeStep> _learnings = new List<CompositeStep>();
        private int _currentLearning;
        private readonly RadioModel _model;

        public Learning(RadioModel model, Action completed, Action<int> stepChanged, Type learningType)
        {
            _completed = completed;
            _stepChanged = stepChanged;
            _model = model;

            _learnings.Add(LearningFactory.CreatePreparationToWorkLearning());
            _learnings.Add(LearningFactory.CreatePreformanceTestLearning());
            _learnings.Add(LearningFactory.CreateSettingFrequencies());

            InitAll();
            FreezeAll();

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

        public void SetCurrentLearning(Type learningType)
        {
            if (learningType == typeof(Preparation))
            {
                _currentLearning = 0;
            }
            else if (learningType == typeof(PerformanceTest))
            {
                _currentLearning = 1;
            }
            else
            {
                _currentLearning = 2;
            }

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
                learning.StartIfInputConditionsAreRight(_model, out var errors);
                learning.Completed += Completed;
                learning.StepChanged += StepChanged;
            }
        }
    }
}
