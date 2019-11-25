using System;
using Windows.UI.Xaml.Data;

namespace LatteLocator.Core.Converters
{
    public class MaskedCCNumberConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null) return "XXXX";

            var stringified = string.Format("{0:0000000000000000}", (long)value);

            return string.Format("XXXX-XXXX-XXXX-{0}", LastFour(stringified));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        private static string LastFour(string value)
        {
            if (string.IsNullOrEmpty(value) || value.Length < 4)
            {
                return "0000";
            }

            return value.Substring(value.Length - 4, 4);
        }
    }
}
