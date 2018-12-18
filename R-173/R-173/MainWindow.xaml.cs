using R_173.Handlers;
using R_173.Interfaces;
using R_173.ViewModels;
using R_173.Views;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using Unity;

namespace R_173
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Dictionary<Type, ITabView> _pages;

        public MainWindow(KeyboardHandler keyboardHandler)
        {
            InitializeComponent();

            var trainingViewModel = new TrainingViewModel();
            var training = new Training(trainingViewModel);
            training.SizeChanged += (s, e) =>
            {
                var attitude = e.NewSize.Width / e.NewSize.Height;
                Console.WriteLine(attitude);
                trainingViewModel.Orientation = attitude > 1.5 ? Orientation.Vertical : Orientation.Horizontal;
            };

            PreviewKeyDown += keyboardHandler.OnPreviewKeyDown;
            PreviewKeyUp += keyboardHandler.OnPreviewKeyUp;

            _pages = new Dictionary<Type, ITabView>
            {
                { typeof(Appointment), new Appointment() },
                { typeof(Tasks), new Tasks(){ DataContext = new TasksViewModel() } },
                { typeof(Training), training },
                { typeof(Work), new Work() { DataContext = new WorkViewModel() } },
            };
            Message.DataContext = App.ServiceCollection.Resolve<IMessageBox>();

            MainContent.Content = _pages[typeof(Appointment)];

            ContentRendered += delegate
            {
                App.ServiceCollection.Resolve<KeyboardHandler>().OnKeyDown += key =>
                {
                    if (key != Key.F1)
                        return;
                    MainContent.Content = new Training2() { DataContext = new TrainingViewModel() };
                };
            };
        }

        public void GoToTaskTab()
        {
            (buttons.Children[2] as RadioButton).IsChecked = true;
            MainContent.Content = _pages[typeof(Tasks)];
        }

        private void ChangeTab(object sender, RoutedEventArgs e)
        {
            var page = (sender as ButtonBase).CommandParameter as Type;
            MainContent.Content = _pages[page];
        }

        private void CloseWelcome(object sender, RoutedEventArgs e)
        {
            Welcome.Visibility = Visibility.Collapsed;
            App.ServiceCollection.Resolve<IMessageBox>().ShowDialog("Hello", "I am radio");
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

    public class NullableToVisibilityConverter : IValueConverter
    {
        public Visibility NullValue { get; set; }
        public Visibility NotNullValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? NullValue : NotNullValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
