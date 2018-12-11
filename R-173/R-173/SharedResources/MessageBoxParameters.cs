using System;

namespace R_173.SharedResources
{
    class MessageBoxParameters
    {
        public readonly Action Ok;
        public readonly Action Cancel;
        public readonly string Title;
        public readonly string Message;
        public readonly string OkText;
        public readonly string CancelText;

        public MessageBoxParameters(Action ok, Action cancel, string title, string message, string okText, string cancelText)
        {
            Ok = ok;
            Cancel = cancel;
            Title = title;
            Message = message;
            OkText = okText;
            CancelText = cancelText;
        }

        public MessageBoxParameters(Action ok, string title, string message, string okText)
        {
            Ok = ok;
            Cancel = null;
            Title = title;
            Message = message;
            OkText = okText;
            CancelText = null;
        }

        public MessageBoxParameters(string title, string message, string okText)
        {
            Ok = null;
            Cancel = null;
            Title = title;
            Message = message;
            OkText = okText;
            CancelText = null;
        }

        public MessageBoxParameters(string title, string message)
        {
            Ok = null;
            Cancel = null;
            Title = title;
            Message = message;
            OkText = "Понятно";
            CancelText = null;
        }
    }
}
