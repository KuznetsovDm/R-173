using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace R_173.Views.Radio
{
    public class Encoder : Image
    {
        private Vector _previousMousePosition;
        private Point _centerImage;

        public Encoder()
        {
            MouseDown += Initialize;
            MouseDown += OnMouseDown;
            MouseUp += OnMouseUp;
            LostMouseCapture += (s, e) => FinishRotate();
        }


        private void Initialize(object sender, MouseButtonEventArgs e)
        {
            MouseDown -= Initialize;
            _centerImage = new Point(Canvas.GetLeft(this) + ActualWidth / 2, Canvas.GetTop(this) + ActualHeight / 2);
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            _previousMousePosition = e.MouseDevice.GetPosition((IInputElement)Parent) - _centerImage;
            MouseMove += OnMouseMove;
            Mouse.Capture(this);
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            FinishRotate();
            Mouse.Capture(null);
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            var currentMousePosition = e.MouseDevice.GetPosition((IInputElement)Parent) - _centerImage;
            double changeAngle = Vector.AngleBetween(_previousMousePosition, currentMousePosition);
            _previousMousePosition = currentMousePosition;
            GetRotate(this)?.Execute(changeAngle);
        }

        void FinishRotate()
        {
            MouseMove -= OnMouseMove;
        }

        #region RotateProperty
        public static readonly DependencyProperty RotateProperty = DependencyProperty.Register(
                "Rotate",
                typeof(ICommand),
                typeof(Encoder),
                new FrameworkPropertyMetadata(null)
            );

        public static void SetRotate(DependencyObject element, ICommand value)
        {
            element.SetValue(RotateProperty, value);
        }

        public static ICommand GetRotate(DependencyObject element)
        {
            return (ICommand)element.GetValue(RotateProperty);
        }
        #endregion

        #region AngleProperty
        public static readonly DependencyProperty AngleProperty = DependencyProperty.Register(
                "Angle",
                typeof(double),
                typeof(Encoder),
                new FrameworkPropertyMetadata(
                    0.0,
                    new PropertyChangedCallback(AngleChanged)
                    )
            );

        public static void SetAngle(DependencyObject element, double value)
        {
            element.SetValue(AngleProperty, value);
        }

        public static double GetAngle(DependencyObject element)
        {
            return (double)element.GetValue(AngleProperty);
        }

        private static void AngleChanged(DependencyObject depObj, DependencyPropertyChangedEventArgs args)
        {
            var image = (Encoder)depObj;
            image.RenderTransform = new RotateTransform(Convert.ToDouble(args.NewValue));
        }
        #endregion
    }
}
