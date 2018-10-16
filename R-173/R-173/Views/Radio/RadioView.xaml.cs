using R_173.Interfaces;
using R_173.ViewModels;
using System;
using System.Globalization;
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

        public RadioView()
        {
            InitializeComponent();

            var radioViewModel = new RadioViewModel();
            DataContext = radioViewModel;

            IsVisibleChanged += (s, e) =>
            {
                var manager = App.ServiceCollection.Resolve<IRadioManager>();
                manager.SetModel((bool)e.NewValue ? radioViewModel.Model : null);
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
            currentFigure.Segments.Add(new LineSegment(point, isStroked: true));
        }

        void EndFigure()
        {
            currentFigure = null;
        }
    }

    public class FrequencyNumberToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return $"/Files/radio/{value.ToString()}.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class FrequencyToImageSourcesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "/Files/radio/0.png";
            var v = value.ToString();
            var param = v.Length - (parameter.ToString()[0] - '0') - 1;
            return param >= v.Length || param < 0 ? "/Files/radio/0.png" : $"/Files/radio/{v[param]}.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
