using System;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Xps.Packaging;

namespace R_173.Views
{
    /// <summary>
    /// Interaction logic for Appointment.xaml
    /// </summary>
    public partial class Appointment : UserControl, ITabView
    {
        public Appointment()
        {
            InitializeComponent();
            var document = new XpsDocument(Properties.Resources.XpsDescriptionPath, FileAccess.Read);
            docViewer.Document = document.GetFixedDocumentSequence();
        }
    }

    public class TabSizeConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            TabControl tabControl = values[0] as TabControl;
            double width = tabControl.ActualWidth / (tabControl.Items.Count) - 1;
            return (width <= 1) ? 0 : (width - 1);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
