using System;
using System.Text.RegularExpressions;
using Xamarin.Forms;

using static HealthApp.Utils.EnumUtility;

namespace HealthApp.Controls.Behaviors.Entries
{
    public class EmailValidatorBehavior : Behavior<Entry>
    {
        private const string EmailRegex = @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";

        private static readonly BindablePropertyKey IsValidPropertyKey = BindableProperty.CreateReadOnly(
            propertyName: nameof(CurrentEmailState),
            returnType: typeof(BehaviorState.EmailEntryState),
            declaringType: typeof(EmailValidatorBehavior),
            defaultValue: BehaviorState.EmailEntryState.None);

        public static readonly BindableProperty IsValidProperty = IsValidPropertyKey.BindableProperty;

        public BehaviorState.EmailEntryState CurrentEmailState
        {
            get => (BehaviorState.EmailEntryState)GetValue(IsValidProperty);
            private set => SetValue(IsValidPropertyKey, value);
        }

        protected override void OnAttachedTo(Entry bindable)
        {
            bindable.TextChanged += HandleTextChanged;

            base.OnAttachedTo(bindable);
        }

        private void HandleTextChanged(object sender, TextChangedEventArgs e)
        {
            if (((Entry)sender).Text.Length > 0)
            {
                bool isValid = Regex.IsMatch(
                    input: e.NewTextValue,
                    pattern: EmailRegex,
                    options: RegexOptions.IgnoreCase,
                    matchTimeout: TimeSpan.FromMilliseconds(250));

                CurrentEmailState = isValid ? BehaviorState.EmailEntryState.Success : BehaviorState.EmailEntryState.Error;
            }
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.TextChanged -= HandleTextChanged;

            base.OnDetachingFrom(bindable);
        }
    }
}
