using System;

namespace R_173.Radio
{
    public enum SwitcherState { Enable, Disable }

    public class Model
    {
        public Model()
        {
            Interference = new Property<SwitcherState>();
            Power = new Property<SwitcherState>();
            Tone = new Property<SwitcherState>();
            Noise = new Property<SwitcherState>();
        }

        /// <summary>
        /// Тумблер "Подавитель помех"
        /// </summary>
        public Property<SwitcherState> Interference;
        /// <summary>
        /// Тумблер "Мощность"
        /// </summary>
        public Property<SwitcherState> Power;
        /// <summary>
        /// Кнопка "Тон"
        /// </summary>
        public Property<SwitcherState> Tone;
        /// <summary>
        /// Тумблер "Подавитель шумов"
        /// </summary>
        public Property<SwitcherState> Noise;
        /// <summary>
        /// Ручка "Громкость ПРМ"
        /// </summary>
        public Property<int> VolumePRM;
        /// <summary>
        /// Тумблер включения питания радиостанции
        /// </summary>
        public Property<SwitcherState> TurningOn;
        /// <summary>
        /// Левый тумблер "ПУ - ОА"
        /// </summary>
        public Property<SwitcherState> LeftPuOa;
        /// <summary>
        /// Правый тумблер "ПУ - ОА"
        /// </summary>
        public Property<SwitcherState> RightPuOa;
        /// <summary>
        /// Ручка регулятора громкости
        /// </summary>
        public Property<int> Volume;
        /// <summary>
        /// Тумблер "Запись - работа"
        /// </summary>
        public Property<SwitcherState> RecordWork;

        /// <summary>
        /// Нажатие на кнопки выбора и подготовки ЗПЧ
        /// </summary>
        /// <param name="number">число</param>
        public void PressNumber(char number)
        {

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

    public class ValueChangedEventArgs<T> : EventArgs
    {
        public readonly T NewValue;
        public ValueChangedEventArgs(T NewValue)
        {
            this.NewValue = NewValue;
        }
    }
}
