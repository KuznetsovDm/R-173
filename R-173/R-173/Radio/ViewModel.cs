using System.ComponentModel;
using System.Windows.Input;
using R_173.ScharedResources;

namespace R_173.Radio
{
    public class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private double _volume;
        private bool _interference;
        private SwitcherState _power;
        private string _frequency;
        private readonly Model _model;

        public ViewModel()
        {
            _model = new Model();
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


        public void Subscribe()
        {

        }

        public void OnPropertyChanged(string prop = "")
        {
            var c = PropertyChanged;
            c?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
