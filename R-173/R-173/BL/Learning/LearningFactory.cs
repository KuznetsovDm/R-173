using R_173.Handlers;
using R_173.Interfaces;
using R_173.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace R_173.BL.Learning
{
    public class LearningFactory
    {
        private KeyboardHandler _keyboardHandler;

        public LearningFactory(KeyboardHandler keyboardHandler)
        {
            _keyboardHandler = keyboardHandler;
        }

        public CompositeStep CreateInitialStateLearning()
        {
            return new CompositeStepBuilder()
                .Add(new InitialStateStep())
                .Build();
        }

        public CompositeStep CreatePreparationToWorkLearning(int? from = null, int? to = null)
        {
            var steps = new IStep<RadioModel>[]
            {
                new WaitingStep(_keyboardHandler),
                new InitialStateStep(),
                new TurningOnStep(
                    checkInputConditions: CheckInitialState,
                    checkInternalState: PreparationLearning.CheckTurningOnInternalState
                    ),
                new ButtonStep(
                    checkInputConditions: PreparationLearning.CheckButtonInputConditions,
                    checkInternalState: PreparationLearning.CheckButtonInternalState
                    ),
                new BoardStep(
                    checkInputConditions: PreparationLearning.CheckButtonInputConditions,
                    checkInternalState: PreparationLearning.CheckButtonInternalState
                    )
            };

            from = from ?? 0;
            to = to ?? steps.Length;

            var builder = new CompositeStepBuilder();

            for (var i = from.Value; i < to.Value; i++)
                builder.Add(steps[i]);

            return builder.Build();
        }

        public CompositeStep CreatePerformanceTestLearning()
        {
            return new CompositeStepBuilder()
                .Add(CreatePreparationToWorkLearning(1)) // todo: только пункты 1-9
                .Add(new WaitingStep(
                    _keyboardHandler,
                    checkInputConditions: PerformanceTestLearning.CheckWorkingState,
                    checkInternalState: PerformanceTestLearning.CheckWorkingState
                    )) // todo: прослушать собственные шумы
                .Add(new VolumeChangeStep(
                    checkInputConditions: PerformanceTestLearning.CheckWorkingState,
                    checkInternalState: PerformanceTestLearning.CheckWorkingState))
                .Add(new NoiseChangedStep(
                    checkInputConditions: PerformanceTestLearning.CheckWorkingState,
                    checkInternalState: PerformanceTestLearning.CheckWorkingState))
                .Add(new PrdPressStep(
                    checkInputConditions: PerformanceTestLearning.CheckWorkingState,
                    checkInternalState: PerformanceTestLearning.CheckWorkingState))
                .Add(new WaitingStep(
                    _keyboardHandler,
                    checkInputConditions: PerformanceTestLearning.CheckWorkingState,
                    checkInternalState: PerformanceTestLearning.CheckWorkingState)) // todo: проверить модуляцию.
                .Add(new WaitingStep(
                    _keyboardHandler,
                    checkInputConditions: PerformanceTestLearning.CheckWorkingState,
                    checkInternalState: PerformanceTestLearning.CheckWorkingState)) // todo: проверить отдачу тока в антенну.
                .Add(new PressToneStep(
                    checkInputConditions: PerformanceTestLearning.CheckWorkingState,
                    checkInternalState: PerformanceTestLearning.CheckWorkingState))
                .Add(new WaitingStep(
                    _keyboardHandler,
                    checkInputConditions: PerformanceTestLearning.CheckWorkingState,
                    checkInternalState: PerformanceTestLearning.CheckWorkingState)) // todo: Повторить операции 2-8 на всех Зпч
                .Build();
        }

        public CompositeStep CreateSettingFrequencies()
        {
            return new CompositeStepBuilder()
                .Add(new WaitingStep(_keyboardHandler)) // todo: записать на планке частоты
                .Add(CreatePreparationToWorkLearning(1)) 
                .Add(new RecordWorkToRecordStep(
                    checkInputConditions: PerformanceTestLearning.CheckWorkingState))
                .Add(new ButtonStep(
                    checkInputConditions: SettingFrequenciesLearning.CheckRecordState,
                    checkInternalState: SettingFrequenciesLearning.CheckRecordState))
                .Add(new ResetStep(
                    checkInputConditions: SettingFrequenciesLearning.CheckRecordState,
                    checkInternalState: SettingFrequenciesLearning.CheckRecordState))
                .Add(new FiveButtonsStep(
                    checkInputConditions: SettingFrequenciesLearning.CheckRecordState,
                    checkInternalState: SettingFrequenciesLearning.CheckRecordState))
                .Add(new WaitingStep(
                    _keyboardHandler,
                    checkInputConditions: SettingFrequenciesLearning.CheckRecordState,
                    checkInternalState: SettingFrequenciesLearning.CheckRecordState)) // todo: набрать все остальные частоты
                .Add(new RecordWorkToWorkStep())
                .Build();
        }

        private static CompositeStep CreateFiveButtonsStep(CheckState checkInputConditions = null, CheckState checkInternalState = null)
        {
            return new CompositeStepBuilder()
                .Add(new ButtonStep(checkInputConditions, checkInternalState))
                .Add(new ButtonStep(checkInputConditions, checkInternalState))
                .Add(new ButtonStep(checkInputConditions, checkInternalState))
                .Add(new ButtonStep(checkInputConditions, checkInternalState))
                .Add(new ButtonStep(checkInputConditions, checkInternalState))
                .Build();
        }

        public static bool CheckInitialState(RadioModel model, out IList<string> errors)
        {
            return CheckState(model, InitialState, out errors);
        }

        public static bool CheckState(RadioModel model, Dictionary<string, Check> state, out IList<string> errors)
        {
            errors = new List<string>();

            foreach (var check in state)
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

        public static Dictionary<string, Check> WorkingState = new Dictionary<string, Check>
        {
            { "RecordWork", new Check(model => model.RecordWork.Value == RecordWorkState.Work, "Переключатель ЗАПИСЬ-РАБОТА должен быть в положении РАБОТА") },
            { "TurningOn", new Check(model => model.TurningOn.Value == SwitcherState.Enabled, "Переключатель ПИТАНИЕ должен быть в положении ВКЛ") },
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
