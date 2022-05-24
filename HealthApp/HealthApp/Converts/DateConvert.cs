using HealthApp.Extensions;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace HealthApp.Converts
{
    public class DateConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTimeOffset)
            {
                var date = (DateTimeOffset)value;

                return date.UtcDateTime.ToRelativeDateString(true);
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
