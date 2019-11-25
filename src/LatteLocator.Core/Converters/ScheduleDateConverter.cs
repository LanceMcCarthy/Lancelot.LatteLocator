using System;
using Windows.UI.Xaml.Data;

namespace LatteLocator.Core.Converters
{
    public class ScheduleDateConverter : IValueConverter
    {
        public bool ConvertToTime { get; set; }
        

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (ConvertToTime)
            {
                var stringTime = value as string;
                DateTime result;
                if (DateTime.TryParse(stringTime, out result))
                {
                    return result.ToString("t");
                }

                return stringTime;
            }
            else
            {
                var stringDate = value as string;
                DateTime result;
                if (DateTime.TryParse(stringDate, out result))
                {
                    return result.DayOfWeek.ToString();
                }

                return stringDate;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
