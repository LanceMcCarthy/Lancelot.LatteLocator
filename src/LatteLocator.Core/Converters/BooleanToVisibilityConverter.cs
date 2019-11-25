using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace LatteLocator.Core.Converters
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public bool Inverted { get; set; }

        public object Convert(object value, Type typeName, object parameter, string language)
        {
            var val = System.Convert.ToBoolean(value);
            if(this.Inverted)
            {
                val = !val;
            }

            if (val)
            {
                return Visibility.Visible;
            }

            return Visibility.Collapsed;
        }


        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
