using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using LatteLocator.Core.Models;

namespace LatteLocator.Core.Converters
{
    public class WifiConverter :IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            if(value == null) return Visibility.Collapsed;
            var array = value as List<Feature>;
            if(array == null) return Visibility.Collapsed;

            return array.Select(item => item.Name.ToLowerInvariant()).Any(lowerVariant => lowerVariant == "wireless hotspot" || lowerVariant == "google wi-fi") 
                ? Visibility.Visible 
                : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            var visiblity = (Visibility)value;

            return visiblity == Visibility.Visible;
        }
    }
}
