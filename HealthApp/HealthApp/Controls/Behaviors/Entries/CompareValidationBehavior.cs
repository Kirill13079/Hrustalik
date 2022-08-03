using HealthApp.Controls.Base;
using HealthApp.Utils;
using Xamarin.Forms;

namespace HealthApp.Controls.Behaviors.Entries
{
    public class CompareValidationBehavior : BaseValidationEntryBehavior<Entry, BehaviorState.PasswordState>
    {
        private BehaviorState.PasswordState _currentState = BehaviorState.PasswordState.None;

        public static BindableProperty TextTestProperty = BindableProperty.Create<CompareValidationBehavior, string>(tc => tc.TextTest, default);

        public CompareValidationBehavior()
        {
            SetCurrentState(_currentState);
        }

        public string TextTest
        {
            get => (string)GetValue(TextTestProperty);
            set => SetValue(TextTestProperty, value);
        }

        protected override void OnHandleTextChanged(object sender, TextChangedEventArgs e)
        {
            bool IsValid = e.NewTextValue == TextTest;

            _currentState = IsValid ? BehaviorState.PasswordState.Success : BehaviorState.PasswordState.ConfirmedError;

            if (((Entry)sender).Text.Length == 0)
            {
                _currentState = BehaviorState.PasswordState.None;
            }

            SetCurrentState(_currentState);
        }
    }
}
