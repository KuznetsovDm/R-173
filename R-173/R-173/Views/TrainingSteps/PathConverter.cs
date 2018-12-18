using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace R_173.Views.TrainingSteps
{
    class PathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null ? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, value.ToString()) : null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
