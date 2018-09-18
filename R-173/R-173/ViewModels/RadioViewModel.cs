using System.Windows.Input;
using R_173.ScharedResources;
using R_173.Models;

namespace R_173.ViewModels
{
    public class RadioViewModel : ViewModelBase
    {
        private double _volume;
        private bool _interference;
        private SwitcherState _power;
        private string _frequency;
        private readonly RadioModel _model;

        public RadioViewModel()
        {
            _model = new RadioModel();
            _model.Power.ValueChanged += (s, e) => OnPropertyChanged(nameof(Power));

            InterferenceClickCommand = new SimpleCommand(() => Interference = !Interference);
            ChangeFrequencyCommand = new SimpleCommand<char>(a => Frequency += a);
            ClearFrequencyCommand = new SimpleCommand(() => Frequency = "");
        }


        public ICommand PowerClickCommand { get; }
        public ICommand InterferenceClickCommand { get; }
        public ICommand ChangeFrequencyCommand { get; }
        public ICommand ClearFrequencyCommand { get; }

        public SwitcherState Power
        {
            get => _model.Power.Value;
            set => _model.Power.Value = value;
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

        public string Frequency
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
    }
}
