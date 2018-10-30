using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace R_173.Views.Radio
{
    public class ClickedImage : ContentControl
    {
        private ICommand _onClick;

        public ClickedImage()
        {
            MouseDown += ClickedImage_MouseDown;
            MouseUp += ClickedImage_MouseUp;
            LostMouseCapture += ClickedImage_LostMouseCapture;
        }

        private void ClickedImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _onClick?.Execute(true);
            Mouse.Capture(this);
        }

        private void ClickedImage_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Mouse.Capture(null);
        }

        private void ClickedImage_LostMouseCapture(object sender, MouseEventArgs e)
        {
            _onClick?.Execute(false);
        }




        #region Clickproperty
        public static readonly DependencyProperty ClickProperty = DependencyProperty.Register(
                "Click",
                typeof(ICommand),
                typeof(ClickedImage),
                new FrameworkPropertyMetadata(null, new PropertyChangedCallback(ClickChanged))
            );

        public static void SetClick(DependencyObject element, ICommand value)
        {
            element.SetValue(ClickProperty, value);
        }

        public static ICommand GetClick(DependencyObject element)
        {
            return (ICommand)element.GetValue(ClickProperty);
        }

        private static void ClickChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ClickedImage)._onClick = (ICommand)e.NewValue;
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
