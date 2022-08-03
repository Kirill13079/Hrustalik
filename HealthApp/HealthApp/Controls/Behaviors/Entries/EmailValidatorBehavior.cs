using System;
using System.Text.RegularExpressions;
using HealthApp.Common;
using HealthApp.Controls.Base;
using HealthApp.Utils;
using Xamarin.Forms;

namespace HealthApp.Controls.Behaviors.Entries
{
    public class EmailValidatorBehavior : BaseValidationEntryBehavior<Entry, BehaviorState.EmailState>
    {
        private BehaviorState.EmailState _currentState = BehaviorState.EmailState.None;

        public EmailValidatorBehavior()
        {
            SetCurrentState(_currentState);
        }

        protected override void OnHandleTextChanged(object sender, TextChangedEventArgs e)
        {
            bool isValid = Regex.IsMatch(
                input: e.NewTextValue.Trim(),
                pattern: Constants.EmailRegex,
                options: RegexOptions.IgnoreCase,
                matchTimeout: TimeSpan.FromMilliseconds(250));

            _currentState = isValid ? BehaviorState.EmailState.Success : BehaviorState.EmailState.Error;

            if (((Entry)sender).Text.Length == 0)
            {
                _currentState = BehaviorState.EmailState.None;
            }

            SetCurrentState(_currentState);
        }
    }
}
