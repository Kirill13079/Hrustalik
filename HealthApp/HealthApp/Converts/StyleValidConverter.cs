using System;
using System.Globalization;
using HealthApp.Utils;
using Xamarin.Forms;

namespace HealthApp.Converts
{
    public class StyleValidConverter : IValueConverter
    {
        public Style ErrorStyle { set; get; }

        public Style SuccessStyle { set; get; }

        public Style NoneStyle { set; get; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Style currentStyleState = NoneStyle;

            if (value is BehaviorState.EmailState emailState)
            {
                switch (emailState)
                {
                    case BehaviorState.EmailState.Error:
                        currentStyleState = ErrorStyle;
                        break;
                    case BehaviorState.EmailState.Success:
                        currentStyleState = SuccessStyle;
                        break;
                    case BehaviorState.EmailState.None:
                        currentStyleState = NoneStyle;
                        break;
                    default:
                        currentStyleState = NoneStyle;
                        break;
                }
            }
            else if (value is BehaviorState.PasswordState passwordSate)
            {
                switch (passwordSate)
                {
                    case BehaviorState.PasswordState.Error:
                        currentStyleState = ErrorStyle;
                        break;
                    case BehaviorState.PasswordState.Success:
                        currentStyleState = SuccessStyle;
                        break;
                    case BehaviorState.PasswordState.None:
                        currentStyleState = NoneStyle;
                        break;
                    case BehaviorState.PasswordState.ConfirmedError:
                        currentStyleState = ErrorStyle;
                        break;
                    default:
                        currentStyleState = NoneStyle;
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
