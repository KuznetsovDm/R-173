using R_173.ViewModels;
using System.Windows.Controls;

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

            DataContext = new TrainingViewModel();
        }
    }
}
