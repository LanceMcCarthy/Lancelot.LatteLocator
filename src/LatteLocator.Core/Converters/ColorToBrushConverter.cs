using System;
using System.Globalization;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace LatteLocator.Core.Converters
{
    public class ColorToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            if (value == null)
                return new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));

            if (value is Color)
                return new SolidColorBrush((Color)value);

            if (value is string)
                return new SolidColorBrush(Parse((string)value));

            throw new NotSupportedException("ColorToBurshConverter only supports converting from Color and String");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            throw new NotSupportedException();
        }

        private static Color Parse(string color)
        {
            var offset = color.StartsWith("#") ? 1 : 0;

            var a = Byte.Parse(color.Substring(0 + offset, 2), NumberStyles.HexNumber);
            var r = Byte.Parse(color.Substring(2 + offset, 2), NumberStyles.HexNumber);
            var g = Byte.Parse(color.Substring(4 + offset, 2), NumberStyles.HexNumber);
            var b = Byte.Parse(color.Substring(6 + offset, 2), NumberStyles.HexNumber);

            return Color.FromArgb(a, r, g, b);
        }
    }
}
