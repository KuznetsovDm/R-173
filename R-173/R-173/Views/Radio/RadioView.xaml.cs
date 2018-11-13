using R_173.Interfaces;
using R_173.ViewModels;
using System.Windows;
using System.Windows.Controls;
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
    }
}
