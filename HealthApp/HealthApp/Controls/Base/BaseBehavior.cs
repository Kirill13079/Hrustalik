using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace HealthApp.Controls.Base
{
    public class BaseBehavior<T> : Behavior<T> where T : BindableObject
    {
        public T AssociatedObject { get; private set; }

        protected override void OnAttachedTo(T bindable)
        {
            base.OnAttachedTo(bindable);

            AssociatedObject = bindable;

            if (bindable.BindingContext != null)
            {
                BindingContext = bindable.BindingContext;
            }

            bindable.BindingContextChanged += OnBindingContextChanged;
        }

        protected override void OnDetachingFrom(T bindable)
        {
            base.OnDetachingFrom(bindable);

            bindable.BindingContextChanged -= OnBindingContextChanged;

            AssociatedObject = null;
        }

        private void OnBindingContextChanged(object sender, EventArgs e)
        {
            OnBindingContextChanged();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            try
            {
                BindingContext = AssociatedObject.BindingContext;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
