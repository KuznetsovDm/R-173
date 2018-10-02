using R_173.Interfaces;
using R_173.ViewModels;
using System.Windows.Controls;
using Unity;

namespace R_173.Views.Radio
{
    /// <summary>
    /// Логика взаимодействия для View.xaml
    /// </summary>
    public partial class RadioView : UserControl
    {
        public RadioView()
        {
            InitializeComponent();

            var viewModel = new RadioViewModel();
            DataContext = viewModel;

            IsVisibleChanged += (s, e) => 
                App.ServiceCollection.Resolve<IRadioManager>().
                SetModel((bool)e.NewValue ? viewModel.Model : null);
        }
    }
}
