using System;
using Windows.UI.Xaml.Data;

namespace LatteLocator.Core.Converters
{
    public class CreditCardNumberConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null) return "no card number";
            return string.Format("{0:0000 0000 0000 0000}", (long)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
