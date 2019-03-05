using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using R_173.Handlers;
using Unity;

namespace R_173.SharedResources
{
    public class MessageBehavior
    {
        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached(
                "IsEnabled",
                typeof(bool),
                typeof(MessageBehavior),
                new FrameworkPropertyMetadata(
                    false, 
                    FrameworkPropertyMetadataOptions.Inherits, 
                    OnIsEnabledChanged));


        public static bool GetIsEnabled(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsEnabledProperty);
        }

        public static void SetIsEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(IsEnabledProperty, value);
        }


        private static void OnIsEnabledChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            //if (obj is Button affirmativeButton && affirmativeButton.Name == "PART_AffirmativeButton")
            //{
            //    var keyboardHandler = App.ServiceCollection.Resolve<KeyboardHandler>();
            //    if (args.NewValue is true)
            //    {
            //        keyboardHandler.OnEnterPressed = () =>
            //        {
            //            affirmativeButton.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
            //        };
            //    }
            //    else
            //    {
            //        keyboardHandler.OnEnterPressed = null;
            //    }
            //}

            //if (obj is Button negativeButton && negativeButton.Name == "PART_NegativeButton")
            //{
            //    var keyboardHandler = App.ServiceCollection.Resolve<KeyboardHandler>();
            //    if (args.NewValue is true)
            //    {
            //        keyboardHandler.OnEnterPressed = () =>
            //        {
            //            negativeButton.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
            //        };
            //    }
            //    else
            //    {
            //        keyboardHandler.OnEnterPressed = null;
            //    }
            //}
            if (obj is Button affirmativeButton && affirmativeButton.Name == "PART_AffirmativeButton")
            {
                if (args.NewValue is true)
                    App.ServiceCollection.Resolve<KeyboardHandler>().AffirmativeButton = affirmativeButton;
                else
                    App.ServiceCollection.Resolve<KeyboardHandler>().AffirmativeButton = null;
            }
            else if (obj is Button negativeButton && negativeButton.Name == "PART_NegativeButton")
            {
                if (args.NewValue is true)
                    App.ServiceCollection.Resolve<KeyboardHandler>().NegativeButton = negativeButton;
                else
                    App.ServiceCollection.Resolve<KeyboardHandler>().NegativeButton = null;
            }
        }
    }
}
