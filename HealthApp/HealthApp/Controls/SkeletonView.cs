using System;
using Xamarin.Forms;

namespace HealthApp.Controls
{
    public class SkeletonView : BoxView
    {
        public SkeletonView()
        {
            Device.StartTimer(TimeSpan.FromSeconds(0.5), () =>
            {
                _ = this.FadeTo(0.5, 750, Easing.BounceOut).ContinueWith((x) =>
                {
                    _ = this.FadeTo(1, 750, Easing.BounceOut);
                });

                return true;
            });
        }
    }
}
