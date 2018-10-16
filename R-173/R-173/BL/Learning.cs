using R_173.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace R_173.BL
{
    public class Learning
    {
        public static bool IsInitialState(RadioModel model, out IList<string> errors)
        {
            errors = new List<string>();

            if (model.Noise.Value != NoiseState.Maximum)
                errors.Add("Noise");

            if (model.Interference.Value != SwitcherState.Disabled)
                errors.Add("Interference");

            if (model.Power.Value != PowerState.Full)
                errors.Add("Power");

            if (model.RecordWork.Value != RecordWorkState.Work)
                errors.Add("Work");

            if (Math.Abs(model.Volume.Value - 0.5) > 0.1)
                errors.Add("Volume");

            if (model.VolumePRM.Value > 0.0)
                errors.Add("VolumePRM");

            return errors.Any();
        }

        public static IList<Predicate<RadioModel>> GetWorkingFrequencyPreparationChecks()
        {
            IList<Predicate<RadioModel>> result = new List<Predicate<RadioModel>>
            {
                // перейти на запись
                model =>
                {
                    return model.RecordWork.Value == RecordWorkState.Record;
                },

                // выбрать номер частоты и нажать СБРОС
                model =>
                {
                    return model.Reset.Value == SwitcherState.Enabled;
                },

                // последовательно нажать 5 цифр


                // перейти на работу
                model =>
                {
                    return model.RecordWork.Value == RecordWorkState.Work;
                },

            };

            return result;
        }
    }

    public interface IStep<T>
    {
        void Start(T model);
        event EventHandler StepCompleted;
        event EventHandler StepCrashed;
    }

}
