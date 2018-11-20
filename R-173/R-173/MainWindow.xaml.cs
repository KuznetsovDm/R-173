﻿using R_173.Handlers;
using R_173.Interfaces;
using R_173.ViewModels;
using R_173.Views;
using System;
using System.Collections.Generic;
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

        public MainWindow()
        {
            InitializeComponent();
            var trainingViewModel = new TrainingViewModel();
            var training = new Training() { DataContext = trainingViewModel };
            training.SizeChanged += (s, e) =>
            {
                var attitude = e.NewSize.Width / e.NewSize.Height;
                Console.WriteLine(attitude);
                trainingViewModel.Orientation = attitude > 1.5 ? Orientation.Vertical : Orientation.Horizontal;
            };

            _pages = new Dictionary<Type, ITabView>
            {
                { typeof(Appointment), new Appointment() },
                { typeof(Tasks), new Tasks(){ DataContext = new TasksViewModel() } },
                { typeof(Training), training },
                { typeof(Work), new Work() { DataContext = new WorkViewModel() } },
            };
            Message.DataContext = App.ServiceCollection.Resolve<IMessageBox>();

            //ContentRendered += delegate
            //{
            //    App.ServiceCollection.Resolve<KeyboardHandler>().OnKeyDown += key =>
            //    {
            //        if (key != Key.F1)
            //            return;
            //        App.ServiceCollection.Resolve<IMessageBox>()
            //        .ShowDialog(() => { MessageBox.Show("ok"); }, () => { MessageBox.Show("cancel"); },
            //            "Вы уверены, что хотите перейти на другой этап? Текущий прогресс будет утерян.",
            //            "Перейти", "Отмена");
            //    };
            //};
        }

        private void ChangeTab(object sender, RoutedEventArgs e)
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
