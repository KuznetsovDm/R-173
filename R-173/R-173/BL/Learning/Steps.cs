using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using R_173.BE;
using R_173.Handlers;
using R_173.Models;
using R_173.SharedResources;

namespace R_173.BL.Learning
{
    public class InitialStateStep : Step
    {
        public InitialStateStep(CheckState checkInputConditions = null, CheckState checkInternalState = null)
            : base(checkInputConditions, checkInternalState)
        {
        }

        public override Message GetErrorDescription()
        {
            var innerMessages = new List<Message>();
            if (!LearningFactory.CheckInitialState(Model, out IList<string> errors))
            {
                errors.ForEach(x => innerMessages.Add(new Message { Header = x }));
            }

            return new Message { Header = "Установка исходного положения", Messages = innerMessages };
        }

        protected override void SomethingChanged()
        {
            base.SomethingChanged();

            if (LearningFactory.CheckInitialState(Model, out IList<string> errors))
            {
                OnStepCompleted();
            }
        }
    }

    public class TurningOnStep : Step
    {
        public TurningOnStep(CheckState checkInputConditions = null, CheckState checkInternalState = null)
            : base(checkInputConditions, checkInternalState)
        {
        }

        public override Message GetErrorDescription()
        {
            return new Message { Header = "Тумблер ПИТАНИЕ не установлен в положение ВКЛ" };
        }

        protected override void TurningOn_ValueChanged(object sender, ValueChangedEventArgs<SwitcherState> e)
        {
            if (e.NewValue == SwitcherState.Enabled)
            {
                OnStepCompleted();
            }
        }
    }

    public class ButtonStep : Step
    {
        public ButtonStep(CheckState checkInputConditions = null, CheckState checkInternalState = null)
            : base(checkInputConditions, checkInternalState)
        {

        }

        public override Message GetErrorDescription()
        {
            return new Message { Header = "КНОПКА не нажата" };
        }

        protected override void Numpad_ValueChanged(object sender, ValueChangedEventArgs<SwitcherState> e, int i)
        {
            if (e.NewValue == SwitcherState.Enabled)
            {
                OnStepCompleted();
            }
        }
    }

    public class FiveButtonsStep : Step
    {
        private int _counter = 0;
        public FiveButtonsStep(CheckState checkInputConditions = null, CheckState checkInternalState = null)
            : base(checkInputConditions, checkInternalState)
        {

        }

        public override bool StartIfInputConditionsAreRight(RadioModel model, out IList<string> errors)
        {
            _counter = 0;
            return base.StartIfInputConditionsAreRight(model, out errors);
        }

        protected override void Numpad_ValueChanged(object sender, ValueChangedEventArgs<SwitcherState> e, int i)
        {
            if (e.NewValue == SwitcherState.Enabled)
            {
                if (_counter++ < 4)
                    return;

                OnStepCompleted();
            }
        }

        protected override void Reset_ValueChanged(object sender, ValueChangedEventArgs<SwitcherState> e)
        {
            if (e.NewValue == SwitcherState.Enabled)
            {
                _counter = 0;
            }
        }
        
        public override Message GetErrorDescription()
        {
            return new Message { Header = "Не нажато 5 КНОПОК" };
        }
    }

    public class BoardStep : Step
    {
        public BoardStep(CheckState checkInputConditions = null, CheckState checkInternalState = null)
            : base(checkInputConditions, checkInternalState)
        {

        }

        public override Message GetErrorDescription()
        {
            return new Message { Header = "Не нажата кнопка ТАБЛО" };
        }

        protected override void Board_ValueChanged(object sender, ValueChangedEventArgs<SwitcherState> e)
        {
            OnStepCompleted();
        }
    }

    public class VolumeChangeStep : Step
    {
        public VolumeChangeStep(CheckState checkInputConditions = null, CheckState checkInternalState = null) : base(checkInputConditions, checkInternalState)
        {
        }

        public override Message GetErrorDescription()
        {
            return new Message { Header = "Не проверен РЕГУЛЯТОР ГРОМКОСТИ" };
        }

        protected override void Volume_ValueChanged(object sender, ValueChangedEventArgs<double> e)
        {
            OnStepCompleted();
        }
    }

    public class NoiseChangedStep : Step
    {
        public NoiseChangedStep(CheckState checkInputConditions = null, CheckState checkInternalState = null) : base(checkInputConditions, checkInternalState)
        {
        }

        public override Message GetErrorDescription()
        {
            return new Message { Header = "Не проверен ПОДАВИТЕЛЬ ШУМОВ" };
        }

        protected override void Noise_ValueChanged(object sender, ValueChangedEventArgs<NoiseState> e)
        {
            OnStepCompleted();
        }
    }

    public class PrdPressStep : Step
    {
        public PrdPressStep(CheckState checkInputConditions = null, CheckState checkInternalState = null) : base(checkInputConditions, checkInternalState)
        {
        }

        public override Message GetErrorDescription()
        {
            return new Message { Header = "Не зажата КНОПКА ПРД" };
        }

        protected override void Sending_ValueChanged(object sender, ValueChangedEventArgs<SwitcherState> e)
        {
            if (e.NewValue == SwitcherState.Enabled)
            {
                OnStepCompleted();
            }
        }
    }

    public class PressToneStep : Step
    {
        public PressToneStep(CheckState checkInputConditions = null, CheckState checkInternalState = null) : base(checkInputConditions, checkInternalState)
        {
        }

        public override Message GetErrorDescription()
        {
            return new Message { Header = "Не нажата КНОПКА ТОН" };
        }

        protected override void Tone_ValueChanged(object sender, ValueChangedEventArgs<SwitcherState> e)
        {
            if (e.NewValue == SwitcherState.Enabled)
            {
                OnStepCompleted();
            }
        }
    }

    public class WaitingStep : Step
    {
        private readonly KeyboardHandler _keyboardHandler;

        public WaitingStep(KeyboardHandler keyboardHandler, CheckState checkInputConditions = null, CheckState checkInternalState = null) : base(checkInputConditions, checkInternalState)
        {
            _keyboardHandler = keyboardHandler;
        }

        public override Message GetErrorDescription()
        {
            throw new NotImplementedException();
        }

        public override void Subscribe(RadioModel radioModel)
        {
            base.Subscribe(radioModel);

            if (_keyboardHandler != null)
            {
                _keyboardHandler.OnKeyDown += OnKeyDown;
            }
        }

        public override void Unsubscribe(RadioModel radioModel)
        {
            base.Unsubscribe(radioModel);

            if (_keyboardHandler != null)
            {
                _keyboardHandler.OnKeyDown -= OnKeyDown;
            }
        }

        private void OnKeyDown(Key key)
        {
            if (key == Key.Enter)
            {
                OnStepCompleted();
            }
        }
    }

    public class RecordWorkToRecordStep : Step
    {
        public RecordWorkToRecordStep(CheckState checkInputConditions = null, CheckState checkInternalState = null)
            : base(checkInputConditions, checkInternalState)
        {

        }

        public override Message GetErrorDescription()
        {
            return new Message { Header = "ТУМБЛЕР ЗАПИСЬ-РАБОТА не установлен в положение ЗАПИСЬ" };

        }

        protected override void RecordWork_ValueChanged(object sender, ValueChangedEventArgs<RecordWorkState> e)
        {
            if (e.NewValue == RecordWorkState.Record)
            {
                OnStepCompleted();
            }
        }
    }

    public class RecordWorkToWorkStep : Step
    {
        public RecordWorkToWorkStep(CheckState checkInputConditions = null, CheckState checkInternalState = null)
            : base(checkInputConditions, checkInternalState)
        {

        }

        public override Message GetErrorDescription()
        {
            return new Message { Header = "ТУМБЛЕР ЗАПИСЬ-РАБОТА не установлен в положение РАБОТА" };
        }

        protected override void RecordWork_ValueChanged(object sender, ValueChangedEventArgs<RecordWorkState> e)
        {
            if (e.NewValue == RecordWorkState.Work)
            {
                OnStepCompleted();
            }
        }
    }

    public class ResetStep : Step
    {
        public ResetStep(CheckState checkInputConditions = null, CheckState checkInternalState = null)
            : base(checkInputConditions, checkInternalState)
        {

        }

        public override Message GetErrorDescription()
        {
            return new Message { Header = "Не нажата КНОПКА СБРОС" };
        }

        protected override void Reset_ValueChanged(object sender, ValueChangedEventArgs<SwitcherState> e)
        {
            if (e.NewValue == SwitcherState.Enabled)
            {
                OnStepCompleted();
            }
        }
    }

    public class AllFrequencySetStep : Step
    {
        public AllFrequencySetStep(CheckState checkInputConditions = null, CheckState checkInternalState = null)
            : base(checkInputConditions, checkInternalState)
        {

        }

        protected override void SomethingChanged()
        {
            base.SomethingChanged();

            if (Model.WorkingFrequencies.All(x => x > 0))
            {
                OnStepCompleted();
            }
        }

        public override Message GetErrorDescription()
        {
            var error = Model.WorkingFrequencies
                .Select((index, x) => new { index, x })
                .Where(x => x.x == 0)
                .First();
            return new Message { Header = $"Для ЧАСТОТЫ под номером {error.index} не установлено значение." };
        }
    }

    public class FrequencySetStep : Step
    {
        public FrequencySetStep(int numpadNumber, int frequency, CheckState checkInputConditions = null, CheckState checkInternalState = null)
            : base(checkInputConditions, checkInternalState)
        {
            NumpadNumber = numpadNumber;
            Frequency = frequency;
        }

        public int NumpadNumber { get; }
        public int Frequency { get; }

        protected override void SomethingChanged()
        {
            base.SomethingChanged();
            if (Model.WorkingFrequencies[NumpadNumber] == Frequency)
            {
                OnStepCompleted();
            }
        }

        public override Message GetErrorDescription()
        {
            return new Message { Header = $"Не установлена заданная частота." };
        }
    }
}
