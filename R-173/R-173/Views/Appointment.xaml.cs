using System;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Navigation;
using System.Windows.Xps.Packaging;
using Unity;
using System.Windows;

namespace R_173.Views
{
	/// <summary>
	/// Interaction logic for Appointment.xaml
	/// </summary>
	public partial class Appointment : ITabView
    {
        public Appointment()
        {
            InitializeComponent();
            var document = new XpsDocument(Properties.Resources.XpsDescriptionPath, FileAccess.Read);
            DocViewer.Document = document.GetFixedDocumentSequence();

            DocViewer.AddHandler(Hyperlink.RequestNavigateEvent, new RequestNavigateEventHandler(OnRequestNavigate));

            IsVisibleChanged += (s, e) => RadioView.RadioIsEnabled = (bool)e.NewValue && Radio.Visibility == Visibility.Visible;
        }

	    private bool _comingFromTraining;
		
        private static MainWindow _mainWindow;
        private static MainWindow MainWindow => _mainWindow ?? (_mainWindow = App.ServiceCollection.Resolve<MainWindow>());

        private void OnRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            if (!int.TryParse(e.Uri.Host, out var number))
                return;
			DocViewer.Visibility = Visibility.Collapsed;
			Radio.Visibility = Visibility.Visible;
			RadioView.SetBlackouts(number - 1);
            MainWindow.Cursor = null;
            RadioView.RadioIsEnabled = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DocViewer.Visibility = Visibility.Visible;
            Radio.Visibility = Visibility.Collapsed;
            RadioView.RadioIsEnabled = false;
        }

        private void ToNextStep_Click(object sender, RoutedEventArgs e)
        {
	        _comingFromTraining = false;
            MainWindow.GoToTrainingTab();
        }

        private void ToNextStep_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
	        if (_comingFromTraining)
	        {
		        ToNextStep.Visibility = Visibility.Visible;
		        Radio.Visibility = Visibility.Collapsed;
			}
	        else
	        {
				ToNextStep.Visibility = e.VerticalOffset > e.ExtentHeight * 0.85
					? Visibility.Visible
					: Visibility.Collapsed;
			}
        }

	    public void ResetComingFromTraining()
	    {
		    _comingFromTraining = false;
		    ToNextStep.Visibility = Visibility.Collapsed;
		}

		public void GoToTraining(string type)
	    {
		    var pageNumber = 0;

		    switch (type)
		    {
			    case "Preparation.Begin":
				    pageNumber = 9;
				    break;
			    case "Performance.Begin":
				    pageNumber = 11;
				    break;
			    case "WorkingFrequency.Begin":
				    pageNumber = 12;
				    break;
		    }

		    if (!DocViewer.CanGoToPage(pageNumber)) return;

		    _comingFromTraining = true;
			DocViewer.GoToPage(pageNumber);
	    }
    }

    public class TabSizeConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
	        if (!(values[0] is TabControl tabControl)) return null;

	        var width = tabControl.ActualWidth / (tabControl.Items.Count) - 1;
	        return (width <= 1) ? 0 : (width - 1);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
