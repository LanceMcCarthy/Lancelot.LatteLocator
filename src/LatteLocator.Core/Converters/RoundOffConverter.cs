using System;
using Windows.UI.Xaml.Data;

namespace LatteLocator.Core.Converters
{
    public class RoundOffConverter : IValueConverter
    {
        public bool UseMetric { get; set; }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value != null)
            {
                //determine if metric is set
                if (UseMetric == true)
                {
                    //convert to metric
                    var metric = (double)value * 1.609344;
                    var trimmed = Math.Round(metric, 2);

                    return string.Format("{0} km", trimmed);
                }
                else
                {
                    var trimmed = Math.Round((double)value, 2);
                    return string.Format("{0} miles", trimmed);
                }
            }
            else
            {
                return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
