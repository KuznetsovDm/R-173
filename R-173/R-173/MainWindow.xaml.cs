//using R_173.BE;
using MahApps.Metro.Controls.Dialogs;
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
using System.Windows.Media;
using Unity;

namespace R_173
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly Dictionary<Type, ITabView> _pages;
        private ButtonBase _lastButton;

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
                { typeof(Appointment), new Appointment(){ DataContext = new AppointmentViewModel() } },
                { typeof(Tasks), new Tasks(){ DataContext = new TasksViewModel() } },
                { typeof(Training), training },
                { typeof(Work), new Work { DataContext = new WorkViewModel() } },
            };
            Message.DataContext = App.ServiceCollection.Resolve<IMessageBox>();

            MainContent.Content = _pages[typeof(Appointment)];

            //this.ShowMetroDialogAsync(new Dialog(this, new MetroDialogSettings()));
            _lastButton = buttons.Children[0] as Button;
        }

        public void GoToTaskTab()
        {
            //((RadioButton) buttons.Children[2]).IsChecked = true;
            MainContent.Content = _pages[typeof(Tasks)];
            SelectButton(buttons.Children[2] as Button);
        }

        private void SelectButton(Button button)
        {
            _lastButton.Background = Brushes.RosyBrown;
            //button.Background = Color.;#FFF7F7F7
            _lastButton = button;
        }

        private void ChangeTab(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            SelectButton(button);
            var page = button.CommandParameter as Type;
            MainContent.Content = _pages[page ?? throw new InvalidOperationException()];
        }

        private void CloseWelcome(object sender, RoutedEventArgs e)
        {
            Welcome.Visibility = Visibility.Collapsed;
            //var option = App.ServiceCollection.Resolve<ActionDescriptionOption>();
            //var text = "";
            //App.ServiceCollection.Resolve<IMessageBox>().ShowDialog("Добро пожаловать", text);
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
