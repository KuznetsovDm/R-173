using R_173.Models;
using System.Collections.Generic;

namespace R_173.BL.Learning
{
    public static class PreparationLearning
    {
        public static bool CheckTurningOnInternalState(RadioModel model, out IList<string> errors)
        {
            var state = new Dictionary<string, Check>(LearningFactory.InitialState);
            state.Remove("TurningOn");

            return LearningFactory.CheckState(model, state, out errors);
        }

        public static bool CheckButtonInternalState(RadioModel model, out IList<string> errors)
        {
            return CheckButtonInputConditions(model, out errors);
        }

        public static bool CheckButtonInputConditions(RadioModel model, out IList<string> errors)
        {
            var state = new Dictionary<string, Check>(LearningFactory.InitialState)
            {
                ["TurningOn"] = new Check(m => m.TurningOn.Value == SwitcherState.Enabled, "Питание должно быть включено"),
            };

            return LearningFactory.CheckState(model, state, out errors);
        }
    }
}
