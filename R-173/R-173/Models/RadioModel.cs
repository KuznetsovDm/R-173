namespace R_173.Models
{
    public enum SwitcherState { Enabled, Disabled }
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
        public readonly Property<RecordWorkState> RecordWork;
        /// <summary>
        /// Список рабочих частот
        /// </summary>
        public readonly int[] WorkingFrequencies;

        public RadioModel()
        {
            Frequency = new Property<int>();
            Interference = new Property<SwitcherState>();
            Power = new Property<SwitcherState>();
            Tone = new Property<SwitcherState>();
            Noise = new Property<NoiseState>();
            VolumePRM = new Property<int>();
            TurningOn = new Property<SwitcherState>();
            LeftPuOa = new Property<SwitcherState>();
            RightPuOa = new Property<SwitcherState>();
            Volume = new Property<int>();
            RecordWork = new Property<RecordWorkState>();
            WorkingFrequencies = new int[WorkingFrequenciesCount];
        }
    }
}
