﻿using System.Windows.Input;
using R_173.SharedResources;
using R_173.Models;

namespace R_173.ViewModels
{
    public class RadioViewModel : ViewModelBase
    {
        public readonly RadioModel Model;

        private int _frequencyNumber;
        private string _frequency;
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
        private bool _sending;
        private bool[] _numpad;

        public RadioViewModel()
        {
            Model = new RadioModel();
            _numpad = new bool[10];

            ChangeFrequencyCommand = new SimpleCommand<string>(s => Model.Numpad[int.Parse(s)].Value = SwitcherState.Enabled);
            //ClearFrequencyCommand = new SimpleCommand(() => Frequency = 0);
            ToneCommand = new SimpleCommand<bool>(value => Tone = value);
            VolumeCommand = new SimpleCommand<double>(value => Volume += value);
            VolumePRMCommand = new SimpleCommand<double>(value => VolumePRM += value);
            Numpad0Command = new SimpleCommand<bool>(value => Numpad0 = value);
            Numpad1Command = new SimpleCommand<bool>(value => Numpad1 = value);
            Numpad2Command = new SimpleCommand<bool>(value => Numpad2 = value);
            Numpad3Command = new SimpleCommand<bool>(value => Numpad3 = value);
            Numpad4Command = new SimpleCommand<bool>(value => Numpad4 = value);
            Numpad5Command = new SimpleCommand<bool>(value => Numpad5 = value);
            Numpad6Command = new SimpleCommand<bool>(value => Numpad6 = value);
            Numpad7Command = new SimpleCommand<bool>(value => Numpad7 = value);
            Numpad8Command = new SimpleCommand<bool>(value => Numpad8 = value);
            Numpad9Command = new SimpleCommand<bool>(value => Numpad9 = value);


            Model.FrequencyNumber.ValueChanged += (s, e) =>
            {
                _frequencyNumber = e.NewValue;
                OnPropertyChanged(nameof(FrequencyNumber));
            };
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
            Model.Sending.ValueChanged += (s, e) =>
            {
                _sending = e.NewValue == SwitcherState.Enabled;
                //OnPropertyChanged(nameof(VolumePRM));
            };

            for (var i = 0; i < 10; i++)
            {
                var num = i;
                var propName = "Numpad" + num.ToString();
                Model.Numpad[i].ValueChanged += (s, e) =>
                {
                    _numpad[num] = e.NewValue == SwitcherState.Enabled;
                    OnPropertyChanged(propName);
                };
            }
        }

        public ICommand ChangeFrequencyCommand { get; }
        public ICommand ClearFrequencyCommand { get; }
        public ICommand ToneCommand { get; }
        public ICommand VolumeCommand { get; }
        public ICommand VolumePRMCommand { get; }
        public ICommand Numpad0Command { get; }
        public ICommand Numpad1Command { get; }
        public ICommand Numpad2Command { get; }
        public ICommand Numpad3Command { get; }
        public ICommand Numpad4Command { get; }
        public ICommand Numpad5Command { get; }
        public ICommand Numpad6Command { get; }
        public ICommand Numpad7Command { get; }
        public ICommand Numpad8Command { get; }
        public ICommand Numpad9Command { get; }


        public int FrequencyNumber
        {
            get => _frequencyNumber;
            set
            {
                Model.FrequencyNumber.Value = value;
            }
        }

        public string Frequency
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
                Model.RecordWork.Value = value ? RecordWorkState.Work : RecordWorkState.Record;
            }
        }

        public bool SendingIsPressed
        {
            get => _sending;
            set
            {
                Model.Sending.Value = value ? SwitcherState.Enabled : SwitcherState.Disabled;
            }
        }

        public bool Numpad0
        {
            get => _numpad[0];
            set
            {
                Model.Numpad[0].Value = value ? SwitcherState.Enabled : SwitcherState.Disabled;
            }
        }

        public bool Numpad1
        {
            get => _numpad[1];
            set
            {
                Model.Numpad[1].Value = value ? SwitcherState.Enabled : SwitcherState.Disabled;
            }
        }

        public bool Numpad2
        {
            get => _numpad[2];
            set
            {
                Model.Numpad[2].Value = value ? SwitcherState.Enabled : SwitcherState.Disabled;
            }
        }

        public bool Numpad3
        {
            get => _numpad[3];
            set
            {
                Model.Numpad[3].Value = value ? SwitcherState.Enabled : SwitcherState.Disabled;
            }
        }

        public bool Numpad4
        {
            get => _numpad[4];
            set
            {
                Model.Numpad[4].Value = value ? SwitcherState.Enabled : SwitcherState.Disabled;
            }
        }

        public bool Numpad5
        {
            get => _numpad[5];
            set
            {
                Model.Numpad[5].Value = value ? SwitcherState.Enabled : SwitcherState.Disabled;
            }
        }

        public bool Numpad6
        {
            get => _numpad[6];
            set
            {
                Model.Numpad[6].Value = value ? SwitcherState.Enabled : SwitcherState.Disabled;
            }
        }

        public bool Numpad7
        {
            get => _numpad[7];
            set
            {
                Model.Numpad[7].Value = value ? SwitcherState.Enabled : SwitcherState.Disabled;
            }
        }

        public bool Numpad8
        {
            get => _numpad[8];
            set
            {
                Model.Numpad[8].Value = value ? SwitcherState.Enabled : SwitcherState.Disabled;
            }
        }

        public bool Numpad9
        {
            get => _numpad[9];
            set
            {
                Model.Numpad[9].Value = value ? SwitcherState.Enabled : SwitcherState.Disabled;
            }
        }
    }
}
