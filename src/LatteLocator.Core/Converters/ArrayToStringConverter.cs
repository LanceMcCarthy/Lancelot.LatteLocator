using System;
using Windows.UI.Xaml.Data;

namespace LatteLocator.Core.Converters
{
    public class ArrayToStringConverter : IValueConverter 
    {

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string address = string.Join(",", value);

            return address;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
