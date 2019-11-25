using LatteLocator.Core.Models;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace LatteLocator.Core.Converters
{
    public class IsStoreOpenConverter : IValueConverter
    {
        public bool IsInverted { get; set; }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var store = value as Store;

            if (store?.Open == null)
                return Visibility.Collapsed;

            if (IsInverted) //for CLOSED icon
            {
                return !store.Open == true ? Visibility.Visible : Visibility.Collapsed;
            }
            else //for OPEN icon
            {
                return store.Open == true ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
