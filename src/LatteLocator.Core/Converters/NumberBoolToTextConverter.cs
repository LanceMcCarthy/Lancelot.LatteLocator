using System;
using Windows.UI.Xaml.Data;

namespace LatteLocator.Core.Converters
{
    public class NumberBoolToTextConverter :IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            var open = (bool?)value;

            if (value == null) return "currently closed";

            return open == true ? "open now" : "currently closed";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            throw new NotImplementedException();
        }
    }
}
