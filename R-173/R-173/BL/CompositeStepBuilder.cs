﻿using R_173.Models;
using R_173.Interfaces;
using System.Collections.Generic;

namespace R_173.BL
{
    public class CompositeStepBuilder
    {
        private List<IStep<RadioModel>> _steps = new List<IStep<RadioModel>>();

        public CompositeStepBuilder Add(IStep<RadioModel> step)
        {
            _steps.Add(step);
            return this;
        }

        public CompositeStep Build()
        {
            return new CompositeStep(_steps);
        }
    }


}