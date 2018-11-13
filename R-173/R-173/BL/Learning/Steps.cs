using System;
using System.Collections.Generic;
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

        protected override void Numpad_ValueChanged(object sender, ValueChangedEventArgs<SwitcherState> e)
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

        protected override void Numpad_ValueChanged(object sender, ValueChangedEventArgs<SwitcherState> e)
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
    }

    public class BoardStep : Step
    {
        public BoardStep(CheckState checkInputConditions = null, CheckState checkInternalState = null)
            : base(checkInputConditions, checkInternalState)
        {

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

        public override void Subscribe(RadioModel radioModel)
        {
            base.Subscribe(radioModel);

            if(_keyboardHandler != null)
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
            if(args.Key == System.Windows.Input.Key.Enter)
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

        protected override void Reset_ValueChanged(object sender, ValueChangedEventArgs<SwitcherState> e)
        {
            if (e.NewValue == SwitcherState.Enabled)
            {
                OnStepCompleted();
            }
        }
    }
}
