using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using MahApps.Metro.Controls.Dialogs;
using R_173.Handlers;
using R_173.ViewModels;
using R_173.Views;

namespace R_173
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		private readonly Dictionary<Type, ITabView> _pages;
		private ButtonBase _lastButton;
		private readonly Brush _brush;
		private readonly Brush _selectedBrush;

		public MainWindow(KeyboardHandler keyboardHandler)
		{
			InitializeComponent();
			_brush = new SolidColorBrush(Color.FromRgb(0xFF, 0xF7, 0xF7));
			_selectedBrush = new SolidColorBrush(Color.FromRgb(0xE0, 0xE0, 0xE0));
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
				{typeof(Appointment), new Appointment {DataContext = new AppointmentViewModel()}},
				{typeof(Tasks), new Tasks {DataContext = new TasksViewModel()}},
				{typeof(Training), training},
				{typeof(Work), new Work {DataContext = new WorkViewModel()}}
			};
			MainContent.Content = _pages[typeof(Appointment)];

			_lastButton = Buttons.Children[0] as Button;
			if (_lastButton != null) _lastButton.Background = _selectedBrush;
		}

		public Action ShowDialog(MessageBoxViewModel viewModel)
		{
			Dialog dialog = null;
			Dispatcher.BeginInvoke(new Action(() =>
			{
				dialog = new Dialog(this, null)
				{
					DataContext = viewModel
				};
				WrapAsyncActionInTaskAndStart(async () =>
					await this.ShowMetroDialogAsync(dialog,
								new MetroDialogSettings()))
					.GetAwaiter()
					.GetResult();
			}));

			return () => WrapAsyncActionInTaskAndStart(async () => await this.HideMetroDialogAsync(dialog))
				.GetAwaiter()
				.GetResult();
		}

		private static Task WrapAsyncActionInTaskAndStart(Action action)
		{
			return Task.Factory.StartNew(action,
				CancellationToken.None,
				TaskCreationOptions.None,
				TaskScheduler.FromCurrentSynchronizationContext());
		}

		public void GoToTaskTab()
		{
			MainContent.Content = _pages[typeof(Tasks)];
			SelectButton(Buttons.Children[2] as Button);
		}

		public void GoToTrainingTab()
		{
			MainContent.Content = _pages[typeof(Training)];
			SelectButton(Buttons.Children[1] as Button);
		}

		public void GoToTrainingDocumentation(string type)
		{
			var page = (Appointment)_pages[typeof(Appointment)];
			MainContent.Content = page;
			SelectButton(Buttons.Children[0] as Button);
			page.GoToTraining(type);
		}

		private void SelectButton(Button button)
		{
			button.Background = _selectedBrush;
			_lastButton.Background = _brush;
			_lastButton = button;
		}

		private void ChangeTab(object sender, RoutedEventArgs e)
		{
			var button = sender as Button;
			SelectButton(button);
			var page = button?.CommandParameter as Type;
			MainContent.Content = _pages[page ?? throw new InvalidOperationException()];
			if(_pages[page] is Appointment appointment)
				appointment.ResetComingFromTraining();
		}

		private void CloseWelcome(object sender, RoutedEventArgs e)
		{
			Welcome.Visibility = Visibility.Collapsed;
		}
	}

	public class TabSizeConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(values[0] is TabControl tabControl)) return 0;

			var width = tabControl.ActualWidth / (tabControl.Items.Count);
			return width <= 1 ? 0 : width - 1;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}