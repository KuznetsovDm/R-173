using System;
using System.Collections.Generic;
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

        //public override bool CheckInputConditions(RadioModel model, out IList<string> errors)
        //{
        //    return LearningFactory.CheckInitialState(model, out errors);
        //}

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
        private double _initVolume;
        public VolumeChangeStep(CheckState checkInputConditions = null, CheckState checkInternalState = null) : base(checkInputConditions, checkInternalState)
        {
            _initVolume = Model.Volume.Value;
        }

        protected override void Volume_ValueChanged(object sender, ValueChangedEventArgs<double> e)
        {
            if(Math.Abs(e.NewValue - _initVolume) > 0.2)
            {
                OnStepCompleted();
            }
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
        public WaitingStep(CheckState checkInputConditions = null, CheckState checkInternalState = null) : base(checkInputConditions, checkInternalState)
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
