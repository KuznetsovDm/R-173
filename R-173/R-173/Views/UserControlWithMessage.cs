using System.Threading;
using System.Threading.Tasks;
using R_173.Interfaces;
using R_173.SharedResources;
using System.Windows.Controls;
using R_173.Helpers;
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
                Task.Factory.StartNew(async () => await MetroMessageBoxHelper.ShowDialog(tab.Message),
                    CancellationToken.None,
                    TaskCreationOptions.None,
                    TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}
