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
            
            Model.Frequency.ValueChanged += (s, e) => 
            {
                _frequency = e.NewValue;
                OnPropertyChanged(nameof(Frequency));
            };
            Model.Interference.ValueChanged += (s, e) => 
            {
                _interference = e.NewValue == SwitcherState.Enabled;
                OnPropertyChanged(nameof(Interference));
            };
            Model.LeftPuOa.ValueChanged += (s, e) =>
            {
                _leftPuOa = e.NewValue == SwitcherState.Enabled;
                OnPropertyChanged(nameof(LeftPuOa));
            };
            Model.Noise.ValueChanged += (s, e) =>
            {
                _noise = e.NewValue == NoiseState.Maximum;
                OnPropertyChanged(nameof(Noise));
            };
            Model.Power.ValueChanged += (s, e) =>
            {
                _power = e.NewValue == SwitcherState.Enabled;
                OnPropertyChanged(nameof(Power));
            };
            Model.RecordWork.ValueChanged += (s, e) =>
            {
                _recordWork = e.NewValue == RecordWorkState.Work;
                OnPropertyChanged(nameof(RecordWork));
            };
            Model.RightPuOa.ValueChanged += (s, e) =>
            {
                _rightPuOa = e.NewValue == SwitcherState.Enabled;
                OnPropertyChanged(nameof(RightPuOa));
            };
            Model.Tone.ValueChanged += (s, e) =>
            {
                _tone = e.NewValue == SwitcherState.Enabled;
                OnPropertyChanged(nameof(Tone));
            };
            Model.TurningOn.ValueChanged += (s, e) =>
            {
                _turningOn = e.NewValue == SwitcherState.Enabled;
                OnPropertyChanged(nameof(TurningOn));
            };
            Model.Volume.ValueChanged += (s, e) =>
            {
                _volume = e.NewValue * 360;
                OnPropertyChanged(nameof(Volume));
            };
            Model.VolumePRM.ValueChanged += (s, e) =>
            {
                _volumePRM = e.NewValue * 360;
                OnPropertyChanged(nameof(VolumePRM));
            };
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
                Model.Frequency.Value = value;
            }
        }

        public bool Interference
        {
            get => _interference;
            set
            {
                Model.Interference.Value = value ? SwitcherState.Enabled : SwitcherState.Disabled;
            }
        }

        public bool Power
        {
            get => _power;
            set
            {
                Model.Power.Value = value ? SwitcherState.Enabled : SwitcherState.Disabled;
            }
        }

        public bool Tone
        {
            get => _tone;
            set
            {
                Model.Tone.Value = value ? SwitcherState.Enabled : SwitcherState.Disabled;
            }
        }

        public bool Noise
        {
            get => _noise;
            set
            {
                Model.Noise.Value = value ? NoiseState.Maximum : NoiseState.Minimum;
            }
        }

        public double VolumePRM
        {
            get => _volumePRM;
            set
            {
                Model.VolumePRM.Value = value / 360;
            }
        }

        public bool TurningOn
        {
            get => _turningOn;
            set
            {
                Model.TurningOn.Value = value ? SwitcherState.Enabled : SwitcherState.Disabled;
            }
        }

        public bool LeftPuOa
        {
            get => _leftPuOa;
            set
            {
                Model.LeftPuOa.Value = value ? SwitcherState.Enabled : SwitcherState.Disabled;
            }
        }

        public bool RightPuOa
        {
            get => _rightPuOa;
            set
            {
                Model.RightPuOa.Value = value ? SwitcherState.Enabled : SwitcherState.Disabled;
            }
        }

        public double Volume
        {
            get => _volume;
            set
            {
                Model.Volume.Value = value / 360;
            }
        }

        public bool RecordWork
        {
            get => _recordWork;
            set
            {
                Model.RecordWork.Value = value ? RecordWorkState.Record : RecordWorkState.Work;
            }
        }
    }
}
