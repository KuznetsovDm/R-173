namespace R_173.Models
{
    public enum SwitcherState { Disabled = 0, Enabled = 1 }
    public enum RecordWorkState { Record, Work }
    public enum NoiseState { Minimum, Maximum }

    public class RadioModel
    {
        /// <summary>
        /// Количество рабочих частот
        /// </summary>
        public const int WorkingFrequenciesCount = 10;
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
        public readonly Property<NoiseState> Noise;
        /// <summary>
        /// Ручка "Громкость ПРМ"
        /// </summary>
        public readonly Property<double> VolumePRM;
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
        public readonly Property<double> Volume;
        /// <summary>
        /// Тумблер "Запись - работа"
        /// </summary>
        public readonly Property<RecordWorkState> RecordWork;
        /// <summary>
        /// Тумблер "ПРД"
        /// </summary>
        public readonly Property<SwitcherState> Sending;
        /// <summary>
        /// Список рабочих частот
        /// </summary>
        public readonly int[] WorkingFrequencies;

        public RadioModel()
        {
            Frequency = new Property<int>(p => p, nameof(Frequency));
            Interference = new Property<SwitcherState>(p => p, nameof(Interference));
            Power = new Property<SwitcherState>(p => p, nameof(Power));
            Tone = new Property<SwitcherState>(p => p, nameof(Tone));
            Noise = new Property<NoiseState>(p => p, nameof(Noise));
            VolumePRM = new Property<double>(p => p < 0 ? 0 : p > 1 ? 1 : p, nameof(VolumePRM));
            TurningOn = new Property<SwitcherState>(p => p, nameof(TurningOn));
            LeftPuOa = new Property<SwitcherState>(p => p, nameof(LeftPuOa));
            RightPuOa = new Property<SwitcherState>(p => p, nameof(RightPuOa));
            Volume = new Property<double>(p => p < 0 ? 0 : p > 1 ? 1 : p, nameof(Volume));
            RecordWork = new Property<RecordWorkState>(p => p, nameof(RecordWork));
            Sending = new Property<SwitcherState>(p => p, nameof(Sending));
            WorkingFrequencies = new int[WorkingFrequenciesCount];
        }
    }
}
