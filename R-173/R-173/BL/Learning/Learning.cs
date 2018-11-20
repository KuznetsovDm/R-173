﻿using R_173.Models;
using R_173.SharedResources;
using R_173.Views.TrainingSteps;
using System;
using System.Collections.Generic;

namespace R_173.BL.Learning
{
    public class LearningBL
    {
        private readonly Action _completed;
        private readonly Action<int> _stepChanged;
        private readonly List<CompositeStep> _learnings = new List<CompositeStep>();
        private int _currentLearning;
        private readonly RadioModel _model;

        public LearningBL(RadioModel model, Action completed, Action<int> stepChanged, StepsTypes learningType)
        {
            _completed = completed;
            _stepChanged = stepChanged;
            _model = model;

            _learnings.Add(LearningFactory.CreatePreparationToWorkLearning());
            _learnings.Add(LearningFactory.CreatePerformanceTestLearning());
            _learnings.Add(LearningFactory.CreateSettingFrequencies());

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
            if (learningType == StepsTypes.Preparation)
            {
                _currentLearning = 0;
            }
            else if (learningType == StepsTypes.PerformanceTest)
            {
                _currentLearning = 1;
            }
            else
            {
                _currentLearning = 2;
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
                learning.StartIfInputConditionsAreRight(_model, out var errors);
                learning.Completed += Completed;
                learning.StepChanged += StepChanged;
            }
        }

        public void Restart()
        {
            var learning = _learnings[_currentLearning];
            learning.Reset();
            learning.StartIfInputConditionsAreRight(_model, out var errors);
            learning.Completed += Completed;
            learning.StepChanged += StepChanged;
        }
    }
}