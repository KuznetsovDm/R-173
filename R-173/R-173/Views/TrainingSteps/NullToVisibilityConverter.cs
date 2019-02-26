using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace R_173.Views.TrainingSteps
{
    public class NullableToVisibilityConverter : IValueConverter
    {
        public Visibility NullValue { get; set; }
        public Visibility NotNullValue { get; set; }

        public NullableToVisibilityConverter()
        {
            NullValue = Visibility.Collapsed;
            NotNullValue = Visibility.Visible;
        }

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
