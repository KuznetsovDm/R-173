using R_173.ScharedResources;
using System;

namespace R_173.Models
{
    public class Property<T> where T : struct
    {
        public event EventHandler<ValueChangedEventArgs<T>> ValueChanged;

        private T _value;
        private readonly string _name;

        public Property(string name = "")
        {
            _name = name;
        }


        public T Value
        {
            get => _value;
            set
            {
                if (value.Equals(_value))
                    return;
                _value = value;
                System.Diagnostics.Trace.WriteLine($"{_name} = {value}");
                ValueChanged?.Invoke(this, new ValueChangedEventArgs<T>(value));
            }
        }
    }
}
