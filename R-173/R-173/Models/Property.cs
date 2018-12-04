using R_173.SharedResources;
using System;

namespace R_173.Models
{
    public class Property<T>
    {
        public event EventHandler<ValueChangedEventArgs<T>> ValueChanged;

        private T _value;
        private readonly Func<T, T, T> _checkValue;
        private readonly Action<T> _onValueChange;
        private readonly string _name;

        public Property(Func<T, T, T> checkValue, string name, Action<T> onValueChange = null)
        {
            _checkValue = checkValue;
            _onValueChange = onValueChange;
            _name = name;
        }


        public T Value
        {
            get => _value;
            set
            {
                var newValue = _checkValue(_value, value);
                if (newValue.Equals(_value))
                    return;
                _value = newValue;
                System.Diagnostics.Trace.WriteLine($"{_name} = {newValue}");
                ValueChanged?.Invoke(this, new ValueChangedEventArgs<T>(newValue));
                _onValueChange(newValue);
            }
        }
    }
}
