using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace LatteLocator.Core.Converters
{
    public class BakeryVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string culture)
        {
return Visibility.Visible;
            //var array = value as List<Feature>;

            //foreach (var item in array)
            //{
            //    if (item.name == "La Boulange")
            //    {
            //        return Visibility.Visible;
            //    }
            //}

            //return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            throw new NotImplementedException("La Boulange convert back error");
        }
    }
}
