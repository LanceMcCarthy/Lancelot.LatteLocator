﻿using System;
using System.Globalization;
using Windows.UI.Xaml.Data;

namespace LatteLocator.Core.Converters
{
    public class StringFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            if (parameter == null && value == null)
                return String.Empty;

            if (parameter == null)
                return value.ToString();

            return String.Format(CultureInfo.CurrentCulture, parameter.ToString(), value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            throw new NotSupportedException();
        }
    }
}
