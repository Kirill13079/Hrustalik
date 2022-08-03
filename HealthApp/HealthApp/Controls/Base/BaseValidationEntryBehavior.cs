using System;
using HealthApp.Extensions;
using Xamarin.Forms;

namespace HealthApp.Controls.Base
{
    public class BaseValidationEntryBehavior<T, V> : BaseBehavior<Entry> 
        where T : Entry where V : Enum
    {
        private static readonly BindablePropertyKey IsValidPropertyKey = BindableProperty.CreateReadOnly(
           propertyName: nameof(T),
           returnType: typeof(V),
           declaringType: typeof(T),
           defaultValue: null);

        private static readonly BindablePropertyKey TextPropertyKey = BindableProperty.CreateReadOnly(
            propertyName: nameof(T),
            returnType: typeof(string),
            declaringType: typeof(T),
            defaultValue: null);

        protected static BindableProperty IsValidProperty = IsValidPropertyKey.BindableProperty;
        protected static BindableProperty TextProperty = TextPropertyKey.BindableProperty;

        public V CurrentState
        {
            get => (V)GetValue(IsValidProperty);
            private set => SetValue(IsValidPropertyKey, value);
        }

        public string CurrentStateText
        {
            get => (string)GetValue(TextProperty);
            private set => SetValue(TextPropertyKey, value);
        }

        protected override void OnAttachedTo(Entry bindable)
        {
            base.OnAttachedTo(bindable);

            bindable.TextChanged += OnHandleTextChanged;
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            base.OnDetachingFrom(bindable);

            bindable.TextChanged -= OnHandleTextChanged;
        }

        protected virtual void OnHandleTextChanged(object sender, TextChangedEventArgs e)
        {

        }

        protected virtual void SetCurrentState(V state)
        {
            CurrentState = state;
            CurrentStateText = state.DisplayName();
        }
    }
}
