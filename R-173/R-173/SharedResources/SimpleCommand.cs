using System;
using System.Windows.Input;

namespace R_173.SharedResources
{
    public class SimpleCommand<T> : ICommand
    {
        private readonly Action<T> onExecute;
        public SimpleCommand(Action<T> onExecute) { this.onExecute = onExecute; }

        public event EventHandler CanExecuteChanged;
        private bool canExecute = true;
        public bool SetCanExecute
        {
            get => canExecute;
            set
            {
                if (canExecute != value)
                {
                    canExecute = value;
                    CanExecuteChanged?.Invoke(this, new EventArgs());
                }
            }
        }
        public bool CanExecute(object parameter) => true;
        public void Execute(object parameter) => onExecute((T)parameter);
    }

    public class SimpleCommand : ICommand
    {
        private readonly Action _onExecute;
        private readonly Func<bool> _canExecute;

        public SimpleCommand(Action onExecute, Func<bool> canExecute = null)
        {
            _onExecute = onExecute;
            _canExecute = canExecute ?? (() => true);
        }

        public event EventHandler CanExecuteChanged;


        public void Execute(object parameter) => _onExecute();
        public bool CanExecute(object parameter) => _canExecute();

        public void OnCanExecuteChanged()
        {
            var e = CanExecuteChanged;
            e?.Invoke(this, new EventArgs());
        }
    }
}
