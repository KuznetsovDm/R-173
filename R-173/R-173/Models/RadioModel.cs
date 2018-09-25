using System;

namespace R_173.Models
{
    public enum SwitcherState { Disabled = 0, Enabled = 1 }

    public class RadioModel
    {
        /// <summary>
        /// Частота
        /// </summary>
        public readonly Property<int> Frequency;
        /// <summary>
        /// Тумблер "Подавитель помех"
        /// </summary>
        public readonly Property<SwitcherState> Interference;
        /// <summary>
        /// Тумблер "Мощность"
        /// </summary>
        public readonly Property<SwitcherState> Power;
        /// <summary>
        /// Кнопка "Тон"
        /// </summary>
        public readonly Property<SwitcherState> Tone;
        /// <summary>
        /// Тумблер "Подавитель шумов"
        /// </summary>
        public readonly Property<SwitcherState> Noise;
        /// <summary>
        /// Ручка "Громкость ПРМ"
        /// </summary>
        public readonly Property<int> VolumePRM;
        /// <summary>
        /// Тумблер включения питания радиостанции
        /// </summary>
        public readonly Property<SwitcherState> TurningOn;
        /// <summary>
        /// Левый тумблер "ПУ - ОА"
        /// </summary>
        public readonly Property<SwitcherState> LeftPuOa;
        /// <summary>
        /// Правый тумблер "ПУ - ОА"
        /// </summary>
        public readonly Property<SwitcherState> RightPuOa;
        /// <summary>
        /// Ручка регулятора громкости
        /// </summary>
        public readonly Property<int> Volume;
        /// <summary>
        /// Тумблер "Запись - работа"
        /// </summary>
        public readonly Property<SwitcherState> RecordWork;

        public RadioModel()
        {
            Frequency = new Property<int>(nameof(Frequency));
            Interference = new Property<SwitcherState>(nameof(Interference));
            Power = new Property<SwitcherState>(nameof(Power));
            Tone = new Property<SwitcherState>(nameof(Tone));
            Noise = new Property<SwitcherState>(nameof(Noise));
            VolumePRM = new Property<int>(nameof(VolumePRM));
            TurningOn = new Property<SwitcherState>(nameof(TurningOn));
            LeftPuOa = new Property<SwitcherState>(nameof(LeftPuOa));
            RightPuOa = new Property<SwitcherState>(nameof(RightPuOa));
            Volume = new Property<int>(nameof(Volume));
            RecordWork = new Property<SwitcherState>(nameof(RecordWork));
        }
    }
}
