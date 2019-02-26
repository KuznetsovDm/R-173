using R_173.Handlers;
using R_173.Interfaces;
using R_173.SharedResources;
using System;
using System.Windows;
using System.Windows.Input;
using Unity;

namespace R_173.ViewModels
{
    class MessageBoxViewModel : ViewModelBase, IMessageBox
    {
        private readonly SimpleCommand _okCommand;
        private readonly SimpleCommand _cancelCommand;
        private Action _ok;
        private Action _cancel;
        private string _title;
        private string _message;
        private string _okText;
        private string _cancelText;
        private bool _visible;
        private UIElement _content;

        public MessageBoxViewModel()
        {
            _okCommand = new SimpleCommand(Ok);
            _cancelCommand = new SimpleCommand(Cancel);

            App.ServiceCollection.Resolve<KeyboardHandler>().OnKeyDown += key =>
            {
                if (key == Key.Enter)
                    Ok();
                else if (key == Key.Escape)
                    Cancel();
            };
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
                if (value == _content)
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
        }

        private void Ok()
        {
            if (OkText == null)
                return;
            OkText = null;
            CancelText = null;
            Visible = false;
            _ok?.Invoke();
        }

        private void Cancel()
        {
            if (CancelText == null)
                return;
            OkText = null;
            CancelText = null;
            Visible = false;
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
        }
    }
}
