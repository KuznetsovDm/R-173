using R_173.Models;
using System.Collections.Generic;

namespace R_173.BL.Learning
{
    public static class PerformanceTestLearning
    {
        public static bool CheckWorkingState(RadioModel model, out IList<string> errors)
        {
            var state = new Dictionary<string, Check>(LearningFactory.WorkingState);
            return LearningFactory.CheckState(model, state, out errors);
        }
    }
}
