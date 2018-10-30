using System.Collections.Generic;
using R_173.Models;
using R_173.SharedResources;

namespace R_173.BL
{
    public class InitialStateStep : Step
    {
        protected override void TurningOn_ValueChanged(object sender, ValueChangedEventArgs<SwitcherState> e)
        {
            System.Diagnostics.Trace.WriteLine("Turning on: " + e.NewValue);

            if (e.NewValue == SwitcherState.Enabled)
            {
                OnStepCompleted();
            }
        }
    }

    public class TurningOnStep : Step
    {
        public override bool CheckInputConditions(RadioModel model, out IList<string> errors)
        {
            return CheckInitialState(model, out errors);
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
        public ButtonStep(CheckState checkState):base(checkState)
        {

        }
        protected override void TurningOn_ValueChanged(object sender, ValueChangedEventArgs<SwitcherState> e)
        {
            if (e.NewValue == SwitcherState.Disabled)
                OnStepCrashed(new List<string> { "Выключил питание!" });
        }
        protected override void Numpad_ValueChanged(object sender, ValueChangedEventArgs<SwitcherState> e)
        {
            System.Diagnostics.Trace.WriteLine("Numpad: " + e.NewValue);
            OnStepCompleted();
        }
    }

    public class BoardStep : Step
    {
        protected override void Board_ValueChanged(object sender, ValueChangedEventArgs<SwitcherState> e)
        {
            System.Diagnostics.Trace.WriteLine("Board: " + e.NewValue);
            OnStepCompleted();
        }
    }
}
