using R_173.SharedResources;
using System;
using System.Windows.Input;

namespace R_173.Models
{
    public class Property<T> where T : struct
    {
        public event EventHandler<ValueChangedEventArgs<T>> ValueChanged;

        private T _value;
        private readonly Func<T, T> _checkValue;
        private readonly string _name;

        public Property(Func<T, T> checkValue, string name = "")
        {
            _checkValue = checkValue;
            _name = name;
        }


        public T Value
        {
            get => _value;
            set
            {
                var newValue = _checkValue(value);
                if (newValue.Equals(_value))
                    return;
                _value = newValue;
                System.Diagnostics.Trace.WriteLine($"{_name} = {newValue}");
                ValueChanged?.Invoke(this, new ValueChangedEventArgs<T>(newValue));
            }
        }
    }
}
