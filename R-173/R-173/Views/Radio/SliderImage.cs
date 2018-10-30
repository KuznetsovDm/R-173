using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace R_173.Views.Radio
{
    public class SliderImage : Image
    {
        public const int MaxIndentValue = 60;

        private double _firstPosition;
        private int _maxIndent;

        public SliderImage()
        {
            MouseDown += OnMouseDown;
            MouseUp += OnMouseUp;
            LostMouseCapture += (s, e) => FinishRotate();
            Canvas.SetLeft(this, 0);
        }


        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            _firstPosition = e.MouseDevice.GetPosition((IInputElement)Parent).X - Canvas.GetLeft(this) + _maxIndent;
            Mouse.Capture(this);
            MouseMove += OnMouseMove;
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            FinishRotate();
            Mouse.Capture(null);
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            var currentMousePosition = e.MouseDevice.GetPosition((IInputElement)Parent).X;
            if (_maxIndent == 0)
            {
                Canvas.SetLeft(this, MaxIndentValue);
                return;
            }
            var difference = _firstPosition - currentMousePosition;
            if (difference > _maxIndent)
                difference = _maxIndent;
            else if (difference < 0)
                difference = 0;
            Canvas.SetLeft(this, _maxIndent - difference);
        }

        void FinishRotate()
        {
            GetSlide(this)?.Execute(Canvas.GetLeft(this) > _maxIndent / 2);
            MouseMove -= OnMouseMove;
        }

        #region SlideProperty
        public static readonly DependencyProperty SlideProperty = DependencyProperty.Register(
                "Slide",
                typeof(ICommand),
                typeof(SliderImage),
                new FrameworkPropertyMetadata(null)
            );

        public static void SetSlide(DependencyObject element, ICommand value)
        {
            element.SetValue(SlideProperty, value);
        }

        public static ICommand GetSlide(DependencyObject element)
        {
            return (ICommand)element.GetValue(SlideProperty);
        }
        #endregion

        #region MaxIndent
        public static readonly DependencyProperty MaxIndentProperty = DependencyProperty.Register(
                "MaxIndent",
                typeof(int),
                typeof(SliderImage),
                new FrameworkPropertyMetadata(0, new PropertyChangedCallback(MaxIndentChanged))
            );

        public static void SetMaxIndent(DependencyObject element, ICommand value)
        {
            element.SetValue(MaxIndentProperty, value);
        }

        public static ICommand GetMaxIndent(DependencyObject element)
        {
            return (ICommand)element.GetValue(MaxIndentProperty);
        }

        private static void MaxIndentChanged(DependencyObject depObj, DependencyPropertyChangedEventArgs args)
        {
            var slider = (SliderImage)depObj;
            var newValue = (int)args.NewValue;
            slider._maxIndent = newValue;
            if (newValue == 0)
                Canvas.SetLeft(slider, MaxIndentValue);
        }
        #endregion
    }
}
