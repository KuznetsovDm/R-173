using System.Windows.Controls;
using Unity;
using R_173.Handlers;
using R_173.ViewModels;
using System.Windows.Input;

namespace R_173.Views
{
    /// <summary>
    /// Interaction logic for Training.xaml
    /// </summary>
    public partial class Training : UserControl, ITabView
    {
        public Training()
        {
            InitializeComponent();

            Loaded += delegate
            {
                App.ServiceCollection.Resolve<KeyboardHandler>().OnKeyDown += key =>
                {
                    if (!Keyboard.IsKeyDown(Key.LeftCtrl))
                        return;
                    var viewModel = DataContext as TrainingViewModel;
                    if (key == Key.Right)
                        viewModel.CurrentStep++;
                    else if (key == Key.Left)
                        viewModel.CurrentStep--;
                };
            };
        }
    }
}
