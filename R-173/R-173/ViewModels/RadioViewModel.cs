using System.Windows.Input;
using R_173.SharedResources;
using R_173.Models;

namespace R_173.ViewModels
{
    public class RadioViewModel : ViewModelBase
    {
        public readonly RadioModel Model;

        private int _frequency;
        private bool _interference;
        private bool _power;
        private bool _tone;
        private bool _noise;
        private double _volumePRM;
        private bool _turningOn;
        private bool _leftPuOa;
        private bool _rightPuOa;
        private double _volume;
        private bool _recordWork;

        public RadioViewModel()
        {
            Model = new RadioModel();

            ChangeFrequencyCommand = new SimpleCommand<string>(s => Frequency = Frequency * 10 + int.Parse(s));
            ClearFrequencyCommand = new SimpleCommand(() => Frequency = 0);
            ButtonCommand = new SimpleCommand<bool>(s => Tone = s);
            VolumeCommand = new SimpleCommand<double>(s => Volume += s);
            VolumePRMCommand = new SimpleCommand<double>(s => VolumePRM += s);
        }

        public ICommand ChangeFrequencyCommand { get; }
        public ICommand ClearFrequencyCommand { get; }
        public ICommand ButtonCommand { get; }
        public ICommand VolumeCommand { get; }
        public ICommand VolumePRMCommand { get; }


        public int Frequency
        {
            get => _frequency;
            set
            {
                if (value == _frequency)
                    return;
                _frequency = value;
                OnPropertyChanged(nameof(Frequency));
            }
        }

        public bool Interference
        {
            get => _interference;
            set
            {
                if (value == _interference)
                    return;
                _interference = value;
                OnPropertyChanged(nameof(Interference));
            }
        }

        public bool Power
        {
            get => _power;
            set
            {
                if (value == _power)
                    return;
                _power = value;
                OnPropertyChanged(nameof(Power));
            }
        }

        public bool Tone
        {
            get => _tone;
            set
            {
                if (value == _tone)
                    return;
                _tone = value;
                OnPropertyChanged(nameof(Tone));
            }
        }

        public bool Noise
        {
            get => _noise;
            set
            {
                if (value == _noise)
                    return;
                _noise = value;
                OnPropertyChanged(nameof(Noise));
            }
        }

        public double VolumePRM
        {
            get => _volumePRM;
            set
            {
                if (value == _volumePRM)
                    return;
                _volumePRM = value;
                OnPropertyChanged(nameof(VolumePRM));
            }
        }

        public bool TurningOn
        {
            get => _turningOn;
            set
            {
                if (value == _turningOn)
                    return;
                _turningOn = value;
                OnPropertyChanged(nameof(TurningOn));
            }
        }

        public bool LeftPuOa
        {
            get => _leftPuOa;
            set
            {
                if (value == _leftPuOa)
                    return;
                _leftPuOa = value;
                OnPropertyChanged(nameof(LeftPuOa));
            }
        }

        public bool RightPuOa
        {
            get => _rightPuOa;
            set
            {
                if (value == _rightPuOa)
                    return;
                _rightPuOa = value;
                OnPropertyChanged(nameof(RightPuOa));
            }
        }

        public double Volume
        {
            get => _volume;
            set
            {
                if (value == _volume)
                    return;
                _volume = value;
                OnPropertyChanged(nameof(Volume));
            }
        }

        public bool RecordWork
        {
            get => _recordWork;
            set
            {
                if (value == _recordWork)
                    return;
                _recordWork = value;
                OnPropertyChanged(nameof(RecordWork));
            }
        }
    }
}
