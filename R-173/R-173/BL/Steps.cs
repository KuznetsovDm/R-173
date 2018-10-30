using System.Collections.Generic;
using R_173.Models;
using R_173.SharedResources;

namespace R_173.BL
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

            IList<string> errors;
            if (LearningFactory.CheckInitialState(Model, out errors))
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

        public override bool CheckInputConditions(RadioModel model, out IList<string> errors)
        {
            return LearningFactory.CheckInitialState(model, out errors);
        }

        protected override void TurningOn_ValueChanged(object sender, ValueChangedEventArgs<SwitcherState> e)
        {
            System.Diagnostics.Trace.WriteLine("Turning on: " + e.NewValue);

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
            System.Diagnostics.Trace.WriteLine("Numpad: " + e.NewValue);
            OnStepCompleted();
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
            System.Diagnostics.Trace.WriteLine("Board: " + e.NewValue);
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
            System.Diagnostics.Trace.WriteLine("Volume: " + e.NewValue);
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
            if (e.NewValue == NoiseState.Minimum)
            {
                OnStepCompleted();
            }
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
        public WaitingStep(CheckState checkInputConditions = null, CheckState checkInternalState = null) : base(checkInputConditions, checkInternalState)
        {
        }

        protected override void Numpad_ValueChanged(object sender, ValueChangedEventArgs<SwitcherState> e)
        {
            OnStepCompleted();
        }
    }
}
