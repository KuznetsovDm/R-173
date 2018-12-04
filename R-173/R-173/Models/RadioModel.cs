using System;
using System.Windows.Media;

namespace R_173.Models
{
    public enum SwitcherState { Disabled = 0, Enabled = 1 }
    public enum RecordWorkState { Record, Work }
    public enum NoiseState { Minimum, Maximum }
    public enum PowerState { Small, Full }

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
        public readonly Property<PowerState> Power;
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
        /// Кнопка "Сброс"
        /// </summary>
        public readonly Property<SwitcherState> Reset;
        /// <summary>
        /// Кнопка "Табло"
        /// </summary>
        public readonly Property<SwitcherState> Board;
        /// <summary>
        /// Индикатор вызова
        /// </summary>
        public readonly Property<Color> CallColor;
        /// <summary>
        /// Индикатор ПРД
        /// </summary>
        public readonly Property<Color> BroadcastColor;
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
            FrequencyNumber = new Property<int>((oldValue, newValue) => newValue, nameof(FrequencyNumber), OnFrequnecyNumberChange);
            Frequency = new Property<string>((oldValue, newValue) => newValue, nameof(Frequency));
            Interference = new Property<SwitcherState>((oldValue, newValue) => newValue, nameof(Interference));
            Power = new Property<PowerState>((oldValue, newValue) => newValue, nameof(Power));
            Tone = new Property<SwitcherState>((oldValue, newValue) => newValue, nameof(Tone));
            Noise = new Property<NoiseState>((oldValue, newValue) => newValue, nameof(Noise));
            VolumePRM = new Property<double>((oldValue, newValue) => newValue < 0 ? 0 : newValue > 1 ? 1 : newValue, nameof(VolumePRM));
            TurningOn = new Property<SwitcherState>((oldValue, newValue) => newValue, nameof(TurningOn), OnTurningOnChange);
            Volume = new Property<double>((oldValue, newValue) => newValue < 0 ? 0 : newValue > 1 ? 1 : newValue, nameof(Volume));
            RecordWork = new Property<RecordWorkState>((oldValue, newValue) => newValue, nameof(RecordWork), OnRecordWorkChange);
            Sending = new Property<SwitcherState>((oldValue, newValue) => newValue, nameof(Sending));
            Reset = new Property<SwitcherState>((oldValue, newValue) => newValue, nameof(Reset), OnResetChange);
            Board = new Property<SwitcherState>(
                (oldValue, newValue) =>
                {
                    return RecordWork.Value == RecordWorkState.Record ? SwitcherState.Enabled : newValue;
                },
                nameof(Board)
            );
            CallColor = new Property<Color>((oldValue, newValue) => newValue, nameof(CallColor));
            BroadcastColor = new Property<Color>((oldValue, newValue) => newValue, nameof(BroadcastColor));

            Numpad = new Property<SwitcherState>[10];
            for (var i = 0; i < 10; i++)
            {
                var num = i;
                Numpad[i] = new Property<SwitcherState>((oldValue, newValue) => newValue,
                    nameof(Numpad) + num.ToString(),
                    value =>
                    {
                        if (value == SwitcherState.Enabled)
                            OnNumpadChange(num);
                    });
            }

            WorkingFrequencies = new int[WorkingFrequenciesCount];
            for (var i = 0; i < WorkingFrequenciesCount; i++)
                WorkingFrequencies[i] = i * i;

            OnFrequnecyNumberChange(0);
            RecordWork.Value = RecordWorkState.Work;
        }

        #region OnChange
        private void OnFrequnecyNumberChange(int state)
        {
            if (TurningOn.Value == SwitcherState.Enabled && Board.Value == SwitcherState.Enabled)
                Frequency.Value = WorkingFrequencies[state].ToString().PadLeft(5, '0');
        }

        private void OnTurningOnChange(SwitcherState state)
        {
            if (TurningOn.Value == SwitcherState.Disabled)
                return;

            if (state == SwitcherState.Enabled)
            {
                FrequencyNumber.Value = 0;
            }
        }

        private void OnRecordWorkChange(RecordWorkState state)
        {
            if (RecordWork.Value == RecordWorkState.Record)
                Board.Value = SwitcherState.Enabled;
            else
                Board.Value = SwitcherState.Disabled;
            _counterNumpadChange = null;
        }

        private void OnBoardChange(SwitcherState state)
        {
            if (Board.Value == SwitcherState.Enabled)
                Frequency.Value = WorkingFrequencies[FrequencyNumber.Value].ToString().PadLeft(5, '0');
            else
                Frequency.Value = "";
        }

        int? _counterNumpadChange;

        private void OnResetChange(SwitcherState state)
        {
            if (TurningOn.Value == SwitcherState.Disabled)
                return;
            if (RecordWork.Value == RecordWorkState.Record)
            {
                Frequency.Value = "";
                _counterNumpadChange = 0;
            }
        }

        private void OnNumpadChange(int i)
        {
            if (TurningOn.Value == SwitcherState.Disabled)
                return;
            if (RecordWork.Value == RecordWorkState.Record && _counterNumpadChange.HasValue)
            {
                Frequency.Value += i.ToString();

                if (++_counterNumpadChange == 5)
                {
                    WorkingFrequencies[FrequencyNumber.Value] = int.Parse(Frequency.Value);
                    _counterNumpadChange = null;
                }

                return;
            }

            FrequencyNumber.Value = i;
            if (RecordWork.Value == RecordWorkState.Record)
                Frequency.Value = WorkingFrequencies[FrequencyNumber.Value].ToString().PadLeft(5, '0');
        }
        #endregion
        public void SetInitialState()
        {
            TurningOn.Value = SwitcherState.Disabled;
            Volume.Value = 0.0;
            RecordWork.Value = RecordWorkState.Work;
            Power.Value = PowerState.Small;
            Interference.Value = SwitcherState.Disabled;
            Noise.Value = NoiseState.Minimum;
            VolumePRM.Value = 0.0;
            for (int i = 0; i < WorkingFrequenciesCount; i++)
            {
                WorkingFrequencies[i] = 0;
            }
        }
    }
}
