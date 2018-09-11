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
        private string _frequency;

        public ViewModel()
        {
            InterferenceClickCommand = new SimpleCommand(() => Interference = !Interference);
            ChangeFrequencyCommand = new SimpleCommand<string>(a => Frequency += a);
            ClearFrequencyCommand = new SimpleCommand(() => Frequency = "");
        }


        public ICommand InterferenceClickCommand { get; }
        public ICommand ChangeFrequencyCommand { get; }
        public ICommand ClearFrequencyCommand { get; }

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


        public void OnPropertyChanged(string prop = "")
        {
            var c = PropertyChanged;
            c?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
