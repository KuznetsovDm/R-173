using System;

namespace R_173.SharedResources
{
    public class EventArgsWithValue<T> : EventArgs
    {
        public T Value { get; set; }
    }
}
