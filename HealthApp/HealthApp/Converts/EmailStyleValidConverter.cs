using System;
using System.Globalization;
using Xamarin.Forms;

using static HealthApp.Utils.EnumUtility;

namespace HealthApp.Converts
{
    public class EmailEntryStyleValidConverter : IValueConverter
    {
        public Style ErrorStyle { set; get; }

        public Style SuccessStyle { set; get; }

        public Style NoneStyle { set; get; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Style currentEmailEntryStyleState = NoneStyle;

            if (value is BehaviorState.EmailEntryState emailState)
            {
                switch (emailState)
                {
                    case BehaviorState.EmailEntryState.Error:
                        currentEmailEntryStyleState = ErrorStyle;
                        break;
                    case BehaviorState.EmailEntryState.Success:
                        currentEmailEntryStyleState = SuccessStyle;
                        break;
                    case BehaviorState.EmailEntryState.None:
                        currentEmailEntryStyleState = NoneStyle;
                        break;
                    default:
                        currentEmailEntryStyleState = NoneStyle;
                        break;
                }
            }

            return currentEmailEntryStyleState;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
