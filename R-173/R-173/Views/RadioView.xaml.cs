using R_173.ViewModels;
using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using R_173.Models;

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

    public class StateToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (SwitcherState)value == SwitcherState.Enabled;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? SwitcherState.Enabled : SwitcherState.Disabled;
        }
    }
}
