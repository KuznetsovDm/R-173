using R_173.Models;
using R_173.SharedResources;
using System;

namespace R_173.BL
{
    public class Learning
    {
        private CompositeStep _learningStep;
        private Action _completed;
        private Action<int> _stepChanged;

        public Learning(RadioModel model, Action completed, Action<int> stepChanged)
        {
            _completed = completed;
            _stepChanged = stepChanged;

            _learningStep = LearningFactory.CreatePreparationToWorkLearning();

            _learningStep.StartIfInputConditionsAreRight(model, out var errors);

            _learningStep.Completed += Completed;
            _learningStep.StepChanged += StepChanged;
        }

        private void Completed(object sender, EventArgs args)
        {
            _completed();
        }

        private void StepChanged(object sender, StepChangedEventArgs args)
        {
            _stepChanged(args.Step);
        }
    }
}
