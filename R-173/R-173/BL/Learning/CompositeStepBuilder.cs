using R_173.Models;
using R_173.Interfaces;
using System.Collections.Generic;

namespace R_173.BL.Learning
{
    public class CompositeStepBuilder
    {
        private readonly List<IStep<RadioModel>> _steps = new List<IStep<RadioModel>>();

        public CompositeStepBuilder Add(IStep<RadioModel> step)
        {
            _steps.Add(step);
            return this;
        }

        public CompositeStep Build(string stepName = null)
        {
            return new CompositeStep(_steps, stepName);
        }
    }
}
