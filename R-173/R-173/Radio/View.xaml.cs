using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace R_173.Radio
{
    /// <summary>
    /// Логика взаимодействия для View.xaml
    /// </summary>
    public partial class View : UserControl
    {
        public View()
        {
            InitializeComponent();

            var viewModel = new ViewModel();
            DataContext = viewModel;
        }
    }

    public class StateToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (State)value == State.Enable;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? State.Enable : State.Disable;
        }
    }
}
