using System;
using System.Globalization;
using Windows.UI.Xaml.Data;

namespace LatteLocator.Core.Converters
{
    public class SimpleTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            return System.Convert.ChangeType(value, targetType, CultureInfo.CurrentCulture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            return System.Convert.ChangeType(value, targetType, CultureInfo.CurrentCulture);
        }
    }
}
