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

            var radioViewModel = new RadioViewModel();
            DataContext = radioViewModel;

            IsVisibleChanged += (s, e) =>
            {
                var manager = App.ServiceCollection.Resolve<IRadioManager>();
                manager.SetModel((bool)e.NewValue ? radioViewModel.Model : null);
            };

        }
    }
}
