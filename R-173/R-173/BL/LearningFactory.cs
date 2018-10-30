﻿using R_173.Interfaces;
using R_173.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace R_173.BL
{
    public static class LearningFactory
    {
        public static CompositeStep CreateInitialStateLearning()
        {
            return new CompositeStepBuilder()
                .Add(new InitialStateStep())
                .Build();
        }

        public static CompositeStep CreatePreparationToWorkLearning()
        {
            return new CompositeStepBuilder()
                .Add(new InitialStateStep())
                .Add(new TurningOnStep(
                    checkInputConditions: CheckInitialState,
                    checkInternalState: PreparationLearning.CheckTurningOnInternalState
                    ))
                .Add(new ButtonStep(
                    checkInputConditions: PreparationLearning.CheckButtonInputConditions,
                    checkInternalState: PreparationLearning.CheckButtonInternalState))
                .Add(new BoardStep(
                    checkInputConditions: PreparationLearning.CheckButtonInputConditions,
                    checkInternalState: PreparationLearning.CheckButtonInternalState))
                .Build();
        }

        public static IStep<RadioModel> CreateSettingFrequencies()
        {
            return new CompositeStepBuilder()
                .Add(new InitialStateStep())
                .Add(new TurningOnStep())
                .Add(new ButtonStep(PreparationLearning.CheckButtonInternalState))
                .Add(new BoardStep())
                .Build();
        }


        public static bool CheckInitialState(RadioModel model, out IList<string> errors)
        {
            errors = new List<string>();

            foreach (var check in InitialState)
            {
                if (!check.Value.CheckMethod(model))
                    errors.Add(check.Value.Error);
            }

            return !errors.Any();
        }

        public static Dictionary<string, Check> InitialState = new Dictionary<string, Check>
        {
            { "Noise", new Check(model => model.Noise.Value == NoiseState.Minimum, "Переключатель ПОДАВИТЕЛЬ ШУМОВ должен быть в положении ВЫКЛ") },
            { "Interference", new Check(model => model.Interference.Value == SwitcherState.Disabled, "Переключатель ПОДАВИТЕЛЬ ПОМЕХ должен быть в положении ВЫКЛ") },
            { "Power", new Check(model => model.Power.Value == PowerState.Full, "Переключатель МОЩНОСТЬ должен быть в положении ПОЛНАЯ") },
            { "RecordWork", new Check(model => model.RecordWork.Value == RecordWorkState.Work, "Переключатель ЗАПИСЬ-РАБОТА должен быть в положении РАБОТА") },
            { "Volume", new Check(model => Math.Abs(model.Volume.Value - 0.5) < 0.1, "Ручка ГРОМКОСТЬ должна быть в среднем положении") },
            { "VolumePRM", new Check(model => model.VolumePRM.Value < 0.01, "Ручка ГРОМКОСТЬ ПРМ должна быть в крайнем левом положении") },
            { "TurningOn", new Check(model => model.TurningOn.Value == SwitcherState.Disabled, "Переключатель ПИТАНИЕ должен быть в положении ВЫКЛ") },
        };
    }

    public class Check
    {
        public Predicate<RadioModel> CheckMethod { get; set; }
        public string Error { get; set; }

        public Check(Predicate<RadioModel> checkMethod, string error)
        {
            CheckMethod = checkMethod;
            Error = error;
        }
    }
}