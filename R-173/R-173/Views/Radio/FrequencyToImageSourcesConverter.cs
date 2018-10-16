using System;
using System.Globalization;
using System.Windows.Data;

namespace R_173.Views.Radio
{
    public class FrequencyToImageSourcesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "/Files/radio/empty.png";
            var v = value.ToString();
            var param = v.Length - (parameter.ToString()[0] - '0') - 1;
            return param >= v.Length || param < 0 ? "/Files/radio/empty.png" : $"/Files/radio/{v[param]}.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
