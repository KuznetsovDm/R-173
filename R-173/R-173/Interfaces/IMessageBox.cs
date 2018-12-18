using R_173.SharedResources;
using System;
using System.Windows;

namespace R_173.Interfaces
{
    interface IMessageBox
    {
        void ShowDialog(Action ok, Action cancel, string title, string message, string okText, string cancelText);
        void ShowDialog(Action ok, string title, string message, string okText);
        void ShowDialog(string title, string message, string okText);
        void ShowDialog(string title, string message);
        void InsertBody(MessageBoxParameters parameters, UIElement element);
        void ShowDialog(MessageBoxParameters parameters);
    }
}
