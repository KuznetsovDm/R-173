using System;

namespace R_173.SharedResources
{
    public class MessageBoxParameters
    {
        public Action Ok { get; set; }
        public Action Cancel { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string OkText { get; set; }
        public string CancelText { get; set; }

        public MessageBoxParameters()
        {
        }

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
