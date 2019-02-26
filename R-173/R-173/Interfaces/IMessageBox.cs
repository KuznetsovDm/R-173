using R_173.SharedResources;
using System;
using System.Windows;

namespace R_173.Interfaces
{
    interface IMessageBox
    {
        void InsertBody(MessageBoxParameters parameters, UIElement element);
        void ShowDialog(MessageBoxParameters parameters);
    }
}
