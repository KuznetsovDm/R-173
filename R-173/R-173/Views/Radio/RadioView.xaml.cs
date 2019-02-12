using R_173.Interfaces;
using R_173.ViewModels;
using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Unity;

namespace R_173.Views.Radio
{
    /// <summary>
    /// Логика взаимодействия для View.xaml
    /// </summary>
    public partial class RadioView : UserControl
    {
        bool isDrawing;

        private static IRadioManager _radioManager;
        private static IRadioManager RadioManager => _radioManager ?? (_radioManager = App.ServiceCollection.Resolve<IRadioManager>());

        public RadioView()
        {
            InitializeComponent();

            IsVisibleChanged += (s, e) =>
            {
                var viewModel = DataContext as RadioViewModel;
                RadioManager.SetModel((bool)e.NewValue ? viewModel?.Model : null);
            };
        }

        void DrawingMouseDown(object sender, MouseButtonEventArgs e)
        {
            Mouse.Capture(DrawingTarget);
            isDrawing = true;
            StartFigure(e.GetPosition(DrawingTarget));
        }

        void DrawingMouseMove(object sender, MouseEventArgs e)
        {
            if (!isDrawing)
                return;
            AddFigurePoint(e.GetPosition(DrawingTarget));
        }

        PathFigure currentFigure;

        void DrawingMouseUp(object sender, MouseButtonEventArgs e)
        {
            AddFigurePoint(e.GetPosition(DrawingTarget));
            EndFigure();
            isDrawing = false;
            Mouse.Capture(null);
        }

        void StartFigure(Point start)
        {
            currentFigure = new PathFigure() { StartPoint = start };
            
            var currentPath =
                new Path()
                {
                    Stroke = Brushes.Black,
                    StrokeThickness = 5,
                    Data = new PathGeometry() { Figures = { currentFigure } }
                };
            currentPath.MouseMove += CurrentPath_MouseMove;
            DrawingTarget.Children.Add(currentPath);
        }

        private void CurrentPath_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.RightButton != MouseButtonState.Pressed)
                return;
            var path = (Path)sender;
            var canvas = path.Parent as Canvas;
            canvas.Children.Remove(path);
        }

        void AddFigurePoint(Point point)
        {
            currentFigure?.Segments.Add(new LineSegment(point, isStroked: true));
        }

        void EndFigure()
        {
            currentFigure = null;
        }

        private void Border_MouseEnter(object sender, MouseEventArgs e)
        {
            var border = sender as Border;
            var ellipse = Ellipses.Children[int.Parse(border.DataContext as string) - 1] as FrameworkElement;

            var viewModel = DataContext as RadioViewModel;
            viewModel.BlackoutIsVisible = true;
            viewModel.BlackoutWidth = ellipse.Width / 2;
            viewModel.BlackoutHeight = ellipse.Height / 2;
            viewModel.BlackoutCenter = new Point(Canvas.GetLeft(ellipse) + viewModel.BlackoutWidth, 
                Canvas.GetTop(ellipse) + viewModel.BlackoutHeight);
            viewModel.BlackoutDescription = BlackoutBehaviour.GetDescription(ellipse);
        }

        private void Border_MouseLeave(object sender, MouseEventArgs e)
        {
            var viewModel = DataContext as RadioViewModel;
            viewModel.BlackoutIsVisible = false;
        }
    }

    public class BlackoutBehaviour
    {
        public static readonly DependencyProperty IsEnabledProperty;
        public static readonly DependencyProperty NumberProperty;
        public static readonly DependencyProperty DescriptionProperty;

        static BlackoutBehaviour()
        {
            IsEnabledProperty = DependencyProperty.RegisterAttached("IsEnabled",
                typeof(bool),
                typeof(BlackoutBehaviour),
                new PropertyMetadata(false, OnIsEnabledChanged));

            NumberProperty = DependencyProperty.RegisterAttached("Number",
                typeof(int),
                typeof(BlackoutBehaviour),
                new PropertyMetadata(0));

            DescriptionProperty = DependencyProperty.RegisterAttached("Description",
                typeof(string),
                typeof(BlackoutBehaviour),
                new PropertyMetadata(null));
        }


        private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ellipse = d as FrameworkElement;
            var width = ellipse.Width / 2;
            var height = ellipse.Height / 2;
            var center = new Point(Canvas.GetLeft(ellipse) + width, Canvas.GetTop(ellipse) + height);

            ellipse.MouseEnter += (s, args) =>
            {
                var viewModel = ellipse.DataContext as RadioViewModel;
                viewModel.BlackoutIsVisible = true;
                viewModel.BlackoutWidth = width;
                viewModel.BlackoutHeight = height;
                viewModel.BlackoutCenter = center;
                viewModel.BlackoutDescription = GetDescription(ellipse);
            };

            ellipse.MouseLeave += (s, args) =>
            {
                var viewModel = ellipse.DataContext as RadioViewModel;
                viewModel.BlackoutIsVisible = false;
            };
        }

        public static bool GetIsEnabled(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsEnabledProperty);
        }

        public static void SetIsEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(IsEnabledProperty, value);
        }

        public static int GetNumber(DependencyObject obj)
        {
            return (int)obj.GetValue(NumberProperty);
        }

        public static void SetNumber(DependencyObject obj, int value)
        {
            obj.SetValue(NumberProperty, value);
        }

        public static string GetDescription(DependencyObject obj)
        {
            return (string)obj.GetValue(DescriptionProperty);
        }

        public static void SetDescription(DependencyObject obj, string value)
        {
            obj.SetValue(DescriptionProperty, value);
        }
    }

    public class MultiBoolToVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values.All(v => v.Equals(true)) ? Visibility.Visible : Visibility.Hidden;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
