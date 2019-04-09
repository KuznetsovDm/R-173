using R_173.SharedResources;
using System.Windows;

namespace R_173.Interfaces
{
	public interface IMessageBox
    {
        void InsertBody(MessageBoxParameters parameters, UIElement element);
        void ShowDialog(MessageBoxParameters parameters);
    }
}
