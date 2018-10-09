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
        /// Номер частоты
        /// </summary>
        public readonly Property<int> FrequencyNumber;
        /// <summary>
        /// Частота
        /// </summary>
        public readonly Property<string> Frequency;
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
        /// Кнопка "Табло"
        /// </summary>
        public readonly Property<SwitcherState> Scoreboard;
        /// <summary>
        /// Кнопка "Сброс"
        /// </summary>
        public readonly Property<SwitcherState> Reset;
        /// <summary>
        /// Кнопки циферблата
        /// </summary>
        public readonly Property<SwitcherState>[] Numpad;
        /// <summary>
        /// Список рабочих частот
        /// </summary>
        public readonly int[] WorkingFrequencies;

        public RadioModel()
        {
            FrequencyNumber = new Property<int>((oldValue, newValue) => newValue, OnFrequnecyNumberChange, nameof(FrequencyNumber));
            Frequency = new Property<string>((oldValue, newValue) => newValue, OnFrequencyChange, nameof(Frequency));
            Interference = new Property<SwitcherState>((oldValue, newValue) => newValue, OnInterferenceChange, nameof(Interference));
            Power = new Property<SwitcherState>((oldValue, newValue) => newValue, OnPowerChange, nameof(Power));
            Tone = new Property<SwitcherState>((oldValue, newValue) => newValue, OnToneChange, nameof(Tone));
            Noise = new Property<NoiseState>((oldValue, newValue) => newValue, OnNoiseChange, nameof(Noise));
            VolumePRM = new Property<double>((oldValue, newValue) => newValue < 0 ? 0 : newValue > 1 ? 1 : newValue, OnVolumePRMChange, nameof(VolumePRM));
            TurningOn = new Property<SwitcherState>((oldValue, newValue) => newValue, OnTurningOnChange, nameof(TurningOn));
            LeftPuOa = new Property<SwitcherState>((oldValue, newValue) => newValue, OnLeftPuOaChange, nameof(LeftPuOa));
            RightPuOa = new Property<SwitcherState>((oldValue, newValue) => newValue, OnRightPuOaChange, nameof(RightPuOa));
            Volume = new Property<double>((oldValue, newValue) => newValue < 0 ? 0 : newValue > 1 ? 1 : newValue, OnVolumeChange, nameof(Volume));
            RecordWork = new Property<RecordWorkState>((oldValue, newValue) => newValue, OnRecordWorkChange, nameof(RecordWork));
            Sending = new Property<SwitcherState>((oldValue, newValue) => newValue, OnSendingChange, nameof(Sending));

            Scoreboard = new Property<SwitcherState>(
                (oldValue, newValue) => RecordWork.Value == RecordWorkState.Record ? oldValue : newValue, 
                OnScoreboardChange, 
                nameof(Scoreboard));

            Reset = new Property<SwitcherState>((oldValue, newValue) => newValue, OnResetChange, nameof(Reset));
            Numpad = new Property<SwitcherState>[10];
            for (var i = 0; i < 10; i++)
            {
                var num = i;
                Numpad[i] = new Property<SwitcherState>((oldValue, newValue) => newValue, () => OnNumpadChange(num), nameof(Numpad) + num.ToString());
            }

            WorkingFrequencies = new int[WorkingFrequenciesCount];

            Frequency.Value = "0";
        }


        private void OnFrequnecyNumberChange()
        {

        }

        private void OnFrequencyChange()
        {

        }

        private void OnInterferenceChange()
        {

        }

        private void OnPowerChange()
        {

        }

        private void OnToneChange()
        {

        }

        private void OnNoiseChange()
        {

        }

        private void OnVolumePRMChange()
        {

        }
        private void OnTurningOnChange()
        {

        }

        private void OnLeftPuOaChange()
        {

        }
        private void OnRightPuOaChange()
        {

        }

        private void OnVolumeChange()
        {

        }

        private void OnRecordWorkChange()
        {
            if (RecordWork.Value == RecordWorkState.Record)
                Scoreboard.Value = SwitcherState.Enabled;
            _counterNumpadChange = null;
        }

        private void OnSendingChange()
        {

        }

        private void OnWorkingFrequenciesChange()
        {

        }

        private void OnScoreboardChange()
        {

        }

        int? _counterNumpadChange;

        private void OnResetChange()
        {
            if (RecordWork.Value == RecordWorkState.Record)
            {
                _counterNumpadChange = 0;
            }
        }

        private void OnNumpadChange(int i)
        {
            if (RecordWork.Value == RecordWorkState.Record && _counterNumpadChange.HasValue)
            {
                Frequency.Value += i.ToString();

                if (++_counterNumpadChange == 4)
                {
                    WorkingFrequencies[FrequencyNumber.Value] = int.Parse(Frequency.Value);
                    _counterNumpadChange = null;
                }

                return;
            }

            FrequencyNumber.Value = i;
        }
    }
}
