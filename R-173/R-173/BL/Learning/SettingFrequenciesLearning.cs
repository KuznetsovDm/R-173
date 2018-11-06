using R_173.Models;
using System.Collections.Generic;

namespace R_173.BL.Learning
{
    public class SettingFrequenciesLearning
    {
        public static bool CheckRecordState(RadioModel model, out IList<string> errors)
        {
            var state = new Dictionary<string, Check>(LearningFactory.WorkingState)
            {
                ["RecordWork"] = new Check(m => m.RecordWork.Value == RecordWorkState.Record, 
                "Переключатель ЗАПИСЬ-РАБОТА должен быть в положении ЗАПИСЬ"),
            };

            return LearningFactory.CheckState(model, state, out errors);
        }
    }
}
