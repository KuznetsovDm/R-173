using System.Windows.Controls;
using Unity;
using R_173.Handlers;
using R_173.ViewModels;
using System.Windows.Input;
using System.Windows;

namespace R_173.Views
{
    /// <summary>
    /// Interaction logic for Training.xaml
    /// </summary>
    public partial class Training : UserControlWithMessage, ITabView
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

    public class MouseEnterBehavior
    {
        //public static readonly DependencyProperty OnMouseEnterProperty =
        //    DependencyProperty.RegisterAttached("OnMouseEnter", typeof(ICommand),
        //        typeof(MouseEnterBehavior), new PropertyMetadata(null));

        //public static ICommand OnMouseEnverCommand(DependencyObject obj)
        //{
        //    return (ICommand)obj.GetValue(OnMouseEnterProperty);
        //}

        public static void SetPageChangedCommand(DependencyObject obj, bool value)
        {
            if (!value)
                return;
            var button = obj as Button;
            button.MouseEnter += delegate { button.Command.Execute(button.ToolTip); };
            //obj.SetValue(OnMouseEnterProperty, value);
        }
    }
}
