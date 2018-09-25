using R_173.ViewModels;
using System.Windows.Controls;

namespace R_173.Views
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
        }
    }
}
