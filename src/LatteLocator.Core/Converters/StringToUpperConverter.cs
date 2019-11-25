﻿using System;
using Windows.UI.Xaml.Data;

namespace LatteLocator.Core.Converters
{
	public class StringToUpperConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string culture)
		{
			return value.ToString().ToUpper();
		}

		public object ConvertBack(object value, Type targetType, object parameter, string culture)
		{
			throw new NotImplementedException();
		}
	}
}
