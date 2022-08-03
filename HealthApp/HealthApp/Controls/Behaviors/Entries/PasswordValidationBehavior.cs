using System.Text.RegularExpressions;
using HealthApp.Common;
using HealthApp.Controls.Base;
using HealthApp.Utils;
using Xamarin.Forms;

namespace HealthApp.Controls.Behaviors.Entries
{
    public class PasswordValidationBehavior : BaseValidationEntryBehavior<Entry, BehaviorState.PasswordState>
    {
        private BehaviorState.PasswordState _currentState = BehaviorState.PasswordState.None;

        public PasswordValidationBehavior()
        {
            SetCurrentState(_currentState);
        }

        protected override void OnHandleTextChanged(object sender, TextChangedEventArgs e)
        {
            base.OnHandleTextChanged(sender, e);

            bool IsValid = Regex.IsMatch(e.NewTextValue, Constants.PasswordRegex);

            _currentState = IsValid ? BehaviorState.PasswordState.Success : BehaviorState.PasswordState.Error;

            if (((Entry)sender).Text.Length == 0)
            {
                _currentState = BehaviorState.PasswordState.None;
            }

            SetCurrentState(_currentState);
        }
    }
}
