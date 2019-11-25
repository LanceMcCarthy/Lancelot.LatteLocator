﻿using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using LatteLocator.Core.Models;

namespace LatteLocator.Core.Converters
{
    public class RewardsVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            var array = value as List<Feature>;

            foreach (var item in array)
            {
                if (item.Name == "Digital Rewards")
                {
                    return Visibility.Visible;
                }
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }
    }
}
