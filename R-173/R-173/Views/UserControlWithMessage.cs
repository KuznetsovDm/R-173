using R_173.Interfaces;
using R_173.SharedResources;
using System.Windows.Controls;
using Unity;

namespace R_173.Views
{
    public class UserControlWithMessage : UserControl
    {
        public UserControlWithMessage()
        {
            IsVisibleChanged += UserControlWithMessage_IsVisibleChanged;
        }

        private void UserControlWithMessage_IsVisibleChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            IsVisibleChanged -= UserControlWithMessage_IsVisibleChanged;

            if (DataContext is ITabWithMessage tab)
                App.ServiceCollection.Resolve<IMessageBox>().ShowDialog(tab.Message);
        }
    }
}
