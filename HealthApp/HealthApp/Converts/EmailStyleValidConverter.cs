using System;
using System.Globalization;
using Xamarin.Forms;

namespace HealthApp.Converts
{
    using static HealthApp.Utils.EnumUtility;

    public class EmailStyleValidConverter : IValueConverter
    {
        public Style Error { set; get; }

        public Style Success { set; get; }

        public Style None { set; get; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Style currentStyleState = None;

            if (value is BehaviorState.EmailEntryState emailState)
            {
                switch (emailState)
                {
                    case BehaviorState.EmailEntryState.Error:
                        currentStyleState = Error;
                        break;
                    case BehaviorState.EmailEntryState.Success:
                        currentStyleState = Success;
                        break;
                    case BehaviorState.EmailEntryState.None:
                        currentStyleState = None;
                        break;
                    default:
                        currentStyleState = None;
                        break;
                }
            }

            return currentStyleState;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
