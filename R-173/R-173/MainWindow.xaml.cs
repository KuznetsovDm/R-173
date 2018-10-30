using R_173.ViewModels;
using R_173.Views;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace R_173
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Dictionary<Type, ITabView> _pages;

        public MainWindow()
        {
            InitializeComponent();

            _pages = new Dictionary<Type, ITabView>
            {
                { typeof(Tasks), new Tasks() },
                { typeof(Appointment), new Appointment() },
                { typeof(Training), new Training(){ DataContext = new TrainingViewModel() } },
                { typeof(Work), new Work() { DataContext = new WorkViewModel() } }
            };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var page = (sender as ButtonBase).CommandParameter as Type;
            MainContent.Content = _pages[page];
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
