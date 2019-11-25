using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace LatteLocator.Core.Converters
{
    public class FeatureVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            //The problemk with this method is that it returns true for all if only one is there.
            List<string> array = (List<string>)value;

            if (array.Contains("Wireless Hotspot") || 
                array.Contains("Sirena Espresso Machine") || 
                array.Contains("Oven-warmed Food") || 
                array.Contains("iTunes WiFi Music Store") ||
                array.Contains("La Boulange"))
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }           
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            var visiblity = (Visibility)value;

            return visiblity == Visibility.Visible;
        }
    }
}
