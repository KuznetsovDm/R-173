using System;
using System.Globalization;
using System.Windows.Data;

namespace R_173.Views.Radio
{
    public class ValueToAngleConverter : IValueConverter
    {
        public const int MaxAngle = 270;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((double)value) * MaxAngle;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
