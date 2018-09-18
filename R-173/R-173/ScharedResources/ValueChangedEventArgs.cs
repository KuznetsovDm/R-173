using System;

namespace R_173.ScharedResources
{
    public class ValueChangedEventArgs<T> : EventArgs
    {
        public readonly T NewValue;
        public ValueChangedEventArgs(T NewValue)
        {
            this.NewValue = NewValue;
        }
    }
}
