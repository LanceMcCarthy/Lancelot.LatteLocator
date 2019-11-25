using System;
using Windows.UI.Xaml.Data;

namespace LatteLocator.Core.Converters
{
    public class StringForBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            if (value == "1") 
                return "Open Now";
            return "Closed";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            throw new NotImplementedException();
        }
    }
}
