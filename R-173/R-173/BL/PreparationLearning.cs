using R_173.Models;
using System.Collections.Generic;
using System.Linq;

namespace R_173.BL
{
    public static class PreparationLearning
    {
        public static bool CheckTurningOnInternalState(RadioModel model, out IList<string> errors)
        {
            var checks = new Dictionary<string, Check>(LearningFactory.InitialState);
            checks.Remove("TurningOn");

            return CheckList(checks, model, out errors);
        }

        public static bool CheckButtonInputConditions(RadioModel model, out IList<string> errors)
        {
            var checks = new Dictionary<string, Check>(LearningFactory.InitialState)
            {
                ["TurningOn"] = new Check(m => m.TurningOn.Value == SwitcherState.Enabled, "Питание должно быть включено"),
            };

            return CheckList(checks, model, out errors);
        }

        public static bool CheckButtonInternalState(RadioModel model, out IList<string> errors)
        {
            return CheckButtonInputConditions(model, out errors);
        }

        private static bool CheckList(Dictionary<string, Check> checks, RadioModel model, out IList<string> errors)
        {
            errors = new List<string>();

            foreach (var check in checks)
            {
                if (!check.Value.CheckMethod(model))
                    errors.Add(check.Value.Error);
            }

            return !errors.Any();
        }
    }
}
