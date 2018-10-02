using R_173.Views;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace R_173
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Dictionary<string, Page> _pages;

        public MainWindow()
        {
            InitializeComponent();

            _pages = new Dictionary<string, Page>
            {
                { nameof(Tasks), new Tasks() },
                { nameof(Appointment), new Appointment() },
                { nameof(Training), new Training() },
                { nameof(Work), new Work() }
            };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var pageName = (sender as Button).CommandParameter.ToString();
            Main_Frame.Content = _pages[pageName];
        }
    }

    public class TabSizeConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            TabControl tabControl = values[0] as TabControl;
            double width = tabControl.ActualWidth / (tabControl.Items.Count);
            return (width <= 1) ? 0 : (width - 1);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
