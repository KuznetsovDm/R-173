using System;
using System.Threading.Tasks;
using MahApps.Metro.Controls.Dialogs;
using R_173.SharedResources;
using Unity;

namespace R_173.Helpers
{
    public class MetroMessageBoxHelper
    {
        public static async Task ShowDialog(MessageBoxParameters parameters)
        {
            var mainWindow = App.ServiceCollection.Resolve<MainWindow>();

            var dialogStyle = string.IsNullOrEmpty(parameters.CancelText)
                ? MessageDialogStyle.Affirmative
                : MessageDialogStyle.AffirmativeAndNegative;

            var result = await mainWindow.ShowMessageAsync(parameters.Title, parameters.Message, dialogStyle,
                new MetroDialogSettings
                {
                    AffirmativeButtonText = parameters.OkText,
                    ColorScheme = MetroDialogColorScheme.Theme,
                    AnimateShow = true,
                    NegativeButtonText = parameters.CancelText,
                    AnimateHide = false,
                    DefaultButtonFocus = MessageDialogResult.Affirmative,
                    DialogResultOnCancel = MessageDialogResult.Canceled,
                });

            if (dialogStyle == MessageDialogStyle.Affirmative)
            {
                if (result == MessageDialogResult.Negative)
                {
                    parameters.Ok?.Invoke();
                }

                return;
            }

            switch (result)
            {
                case MessageDialogResult.Negative:
                    parameters.Cancel?.Invoke();
                    break;
                case MessageDialogResult.Affirmative:
                    parameters.Ok?.Invoke();
                    break;
                case MessageDialogResult.Canceled:
                    break;
                case MessageDialogResult.FirstAuxiliary:
                    break;
                case MessageDialogResult.SecondAuxiliary:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
