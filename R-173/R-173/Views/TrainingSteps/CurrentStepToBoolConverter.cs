using System;
using System.Globalization;
using System.Windows.Data;

namespace R_173.Views.TrainingSteps
{
    class CurrentStepToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var currentStepNumber = (int)value;
            var stepNumber = int.Parse(parameter.ToString());
            if (stepNumber == currentStepNumber)
                return null;
            return stepNumber < currentStepNumber;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
