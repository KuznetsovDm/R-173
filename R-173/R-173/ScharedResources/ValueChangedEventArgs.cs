using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
