using R_173.SharedResources;
using System;
using System.Windows.Input;

namespace R_173.ViewModels
{
    public class TaskViewModel : ViewModelBase
    {
        private int _numberOfAttempts;
        private int _numberOfSuccessfulAttempts;

        public TaskViewModel(string title, Action start)
        {
            Title = title;
            StartCommand = new SimpleCommand(start);
            ShowToolTip = new SimpleCommand<object>(obj => Console.WriteLine(obj.ToString()));
        }


        public int NumberOfAttempts
        {
            get => _numberOfAttempts;
            set
            {
                if (value == _numberOfAttempts)
                    return;
                _numberOfAttempts = value;
                OnPropertyChanged(nameof(NumberOfAttempts));
            }
        }

        public int NumberOfSuccessfulAttempts
        {
            get => _numberOfSuccessfulAttempts;
            set
            {
                if (value == _numberOfSuccessfulAttempts)
                    return;
                _numberOfSuccessfulAttempts = value;
                OnPropertyChanged(nameof(NumberOfSuccessfulAttempts));
            }
        }

        public string Title { get; }
        public ICommand StartCommand { get; }
        public ICommand ShowToolTip { get; }
    }
}
