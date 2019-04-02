using R_173.Interfaces;
using R_173.SharedResources;
using System;
using System.Windows;
using System.Windows.Input;

namespace R_173.ViewModels
{
    public class MessageBoxViewModel : ViewModelBase, IMessageBox
    {
        private readonly SimpleCommand _okCommand;
        private readonly SimpleCommand _cancelCommand;
        private readonly MainWindow _mainWindow;
        private Action _ok;
        private Action _cancel;
        private Action _close;
        private string _title;
        private string _message;
        private string _okText;
        private string _cancelText;
        private bool _visible;
        private UIElement _content;

        public MessageBoxViewModel(MainWindow mainWindow)
        {
            _okCommand = new SimpleCommand(Ok);
            _cancelCommand = new SimpleCommand(Cancel);
            _mainWindow = mainWindow;
        }


        public ICommand OkCommand => _okCommand;
        public ICommand CancelCommand => _cancelCommand;

        public string OkText
        {
            get => _okText;
            set
            {
                if (value == _okText)
                    return;
                _okText = value;
                OnPropertyChanged(nameof(OkText));
            }
        }

        public string CancelText
        {
            get => _cancelText;
            set
            {
                if (value == _cancelText)
                    return;
                _cancelText = value;
                OnPropertyChanged(nameof(CancelText));
            }
        }

        public string Message
        {
            get => _message;
            set
            {
                if (value == _message)
                    return;
                _message = value;
                OnPropertyChanged(nameof(Message));
            }
        }

        public string Title
        {
            get => _title;
            set
            {
                if (value == _title)
                    return;
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        public bool Visible
        {
            get => _visible;
            set
            {
                if (value == _visible)
                    return;
                _visible = value;
                OnPropertyChanged(nameof(Visible));
            }
        }

        public UIElement Content
        {
            get => _content;
            set
            {
                if (Equals(value, _content))
                    return;
                _content = value;
                OnPropertyChanged(nameof(Content));
            }
        }

        public void ShowDialog(MessageBoxParameters parameters)
        {
            Content = null;
            _ok = parameters.Ok;
            _cancel = parameters.Cancel;
            Message = parameters.Message;
            Title = parameters.Title;
            OkText = parameters.OkText;
            CancelText = parameters.CancelText;
            Visible = true;
            _close = _mainWindow.ShowDialog(this);
        }

        private void Ok()
        {
            _close?.Invoke();
            if (OkText == null)
                return;
            _ok?.Invoke();
        }

        private void Cancel()
        {
            _close?.Invoke();
            if (CancelText == null)
                return;
            _cancel?.Invoke();
        }

        public void InsertBody(MessageBoxParameters parameters, UIElement element)
        {
            Content = element;
            _ok = parameters.Ok;
            _cancel = parameters.Cancel;
            Message = parameters.Message;
            Title = parameters.Title;
            OkText = parameters.OkText;
            CancelText = parameters.CancelText;
            Visible = true;
            _close = _mainWindow.ShowDialog(this);
        }
    }
}
