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
        private List<CompositeStep> _learnings;
        private int _currentLearning;
        private RadioModel _model;

        public Learning(RadioModel model, Action completed, Action<int> stepChanged, Type learningType)
        {
            _completed = completed;
            _stepChanged = stepChanged;
            _model = model;

            _learnings[0] = LearningFactory.CreatePreparationToWorkLearning();
            _learnings[1] = LearningFactory.CreatePreparationToWorkLearning();
            _learnings[2] = LearningFactory.CreatePreparationToWorkLearning();

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
        }

        public void Start()
        {
            _learnings[_currentLearning].StartIfInputConditionsAreRight(_model, out var errors);
            _learnings[_currentLearning].Completed += Completed;
            _learnings[_currentLearning].StepChanged += StepChanged;
        }
    }
}
