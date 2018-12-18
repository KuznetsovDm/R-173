using System.Windows.Controls;
using Unity;
using R_173.Handlers;
using R_173.ViewModels;
using System.Windows.Input;
using System;
using System.Windows.Media;
using System.Windows;

namespace R_173.Views
{
    /// <summary>
    /// Interaction logic for Training.xaml
    /// </summary>
    public partial class Training : UserControlWithMessage, ITabView
    {
        private readonly TrainingViewModel _viewModel;

        public Training(TrainingViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;
            DataContext = viewModel;

            Loaded += delegate
            {
                App.ServiceCollection.Resolve<KeyboardHandler>().OnKeyDown += key =>
                {
                    if (!Keyboard.IsKeyDown(Key.LeftCtrl))
                        return;
                    if (key == Key.Right)
                        viewModel.CurrentStep++;
                    else if (key == Key.Left)
                        viewModel.CurrentStep--;
                };
            };
        }

        private void Viewbox_MouseMove(object sender, MouseEventArgs e)
        {
            var viewBox = sender as Viewbox;
            var result = VisualTreeHelper.HitTest(viewBox, e.GetPosition(viewBox));
            _viewModel.CurrentToolTip = result?.VisualHit is FrameworkElement element
                ? element.ToolTip
                : null;
        }

        private void Viewbox_MouseLeave(object sender, MouseEventArgs e)
        {
            _viewModel.CurrentToolTip = null;
        }
    }
}
