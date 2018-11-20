using System;
using System.Collections.Generic;
using System.Text;
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

        public override string GetErrorDescription()
        {
            StringBuilder builder = new StringBuilder();
            if (LearningFactory.CheckInitialState(Model, out IList<string> errors))
            {
                errors.ForEach(x => builder.AppendLine(x));
            }

            return builder.ToString();
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

        public override string GetErrorDescription()
        {
            return "Тумблер ПИТАНИЕ не установлен в положение ВКЛ";
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

        public override string GetErrorDescription()
        {
            return "КНОПКА не нажата";
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

        public override string ToString()
        {
            return base.ToString();
        }

        public override string GetErrorDescription()
        {
            return "Не нажато 5 КНОПОК";
        }
    }

    public class BoardStep : Step
    {
        public BoardStep(CheckState checkInputConditions = null, CheckState checkInternalState = null)
            : base(checkInputConditions, checkInternalState)
        {

        }

        public override string GetErrorDescription()
        {
            return "Не нажата кнопка ТАБЛО";
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

        public override string GetErrorDescription()
        {
            return "Не проверен РЕГУЛЯТОР ГРОМКОСТИ";
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

        public override string GetErrorDescription()
        {
            return "Не проверен ПОДАВИТЕЛЬ ШУМОВ";
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

        public override string GetErrorDescription()
        {
            return "Не зажата КНОПКА ПРД";
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

        public override string GetErrorDescription()
        {
            return "Не нажата КНОПКА ТОН";
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

        public override string GetErrorDescription()
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

        private void OnKeyDown(object sender, KeyEventArgs args)
        {
            if (args.Key == System.Windows.Input.Key.Enter)
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

        public override string GetErrorDescription()
        {
            return "ТУМБЛЕР ЗАПИСЬ-РАБОТА не установлен в положение ЗАПИСЬ";

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

        public override string GetErrorDescription()
        {
            return "ТУМБЛЕР ЗАПИСЬ-РАБОТА не установлен в положение РАБОТА";
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

        public override string GetErrorDescription()
        {
            return "Не нажата КНОПКА СБРОС";
        }

        protected override void Reset_ValueChanged(object sender, ValueChangedEventArgs<SwitcherState> e)
        {
            if (e.NewValue == SwitcherState.Enabled)
            {
                OnStepCompleted();
            }
        }
    }
}
