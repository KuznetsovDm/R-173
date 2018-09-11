using System;

namespace R_173.Radio
{
    public class Model
    {
        public Switcher Power = new Switcher();
    }

    public enum State { Enable, Disable }

    public class ValueChangedEventArgs<T> : EventArgs
    {
        public readonly T NewValue;
        public ValueChangedEventArgs(T NewValue)
        {
            this.NewValue = NewValue;
        }
    }

    public class Property<T> where T : struct
    {
        public event EventHandler<ValueChangedEventArgs<T>> ValueChanged;

        private T _value;

        public T Value
        {
            get => _value;
            set
            {
                if (value.Equals(_value))
                    return;
                _value = value;
                ValueChanged?.Invoke(this, new ValueChangedEventArgs<T>(value));
            }
        }
    }

    public class Switcher : Property<State>
    {
    }
}
