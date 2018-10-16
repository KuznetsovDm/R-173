using System;
using System.Globalization;
using System.Windows.Data;

namespace R_173.Views.Radio
{
    public class FrequencyNumberToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return $"/Files/radio/{value.ToString()}.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
