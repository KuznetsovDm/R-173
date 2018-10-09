using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace R_173.Views.Radio
{
    public class ClickedImage : Image
    {
        public ClickedImage()
        {
            MouseDown += (s, e) => GetClick(this)?.Execute(true);
            MouseUp += (s, e) => GetClick(this)?.Execute(false);
        }


        #region Clickproperty
        public static readonly DependencyProperty ClickProperty = DependencyProperty.Register(
                "Click",
                typeof(ICommand),
                typeof(ClickedImage),
                new FrameworkPropertyMetadata(null)
            );

        public static void SetClick(DependencyObject element, ICommand value)
        {
            element.SetValue(ClickProperty, value);
        }

        public static ICommand GetClick(DependencyObject element)
        {
            return (ICommand)element.GetValue(ClickProperty);
        }
        #endregion

        #region IsDownProperty
        public static readonly DependencyProperty IsDownProperty = DependencyProperty.Register(
                "IsDown",
                typeof(bool),
                typeof(ClickedImage),
                new FrameworkPropertyMetadata(false)
            );

        public static void SetIsDown(DependencyObject element, bool value)
        {
            element.SetValue(ClickProperty, value);
        }

        public static bool GetIsDown(DependencyObject element)
        {
            return (bool)element.GetValue(ClickProperty);
        }
        #endregion
    }
}
