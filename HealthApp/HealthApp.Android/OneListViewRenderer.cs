using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms.Platform.Android;

namespace HealthApp.Droid
{
    public class OneListViewRenderer : ListViewRenderer
    {
        public OneListViewRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.ListView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                Control.Touch -= Control_Touch;
            }

            if (e.NewElement != null)
            {
                Control.Touch += Control_Touch;
            }

            //this.Control.SetSelector(Resource.Drawable.list_item_selector);
        }

        private void Control_Touch(object sender, TouchEventArgs e)
        {
            switch (e.Event.Action)
            {
                case MotionEventActions.Down:
                    // Disallow ScrollView to intercept touch events.
                    this.Parent.RequestDisallowInterceptTouchEvent(true);
                    break;

                case MotionEventActions.Up:
                    // Allow ScrollView to intercept touch events.
                    Parent.RequestDisallowInterceptTouchEvent(false);
                    break;
            }

            Control.OnTouchEvent(e.Event);
        }
    }
}