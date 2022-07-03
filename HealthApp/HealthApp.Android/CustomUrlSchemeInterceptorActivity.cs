using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using HealthApp.Common;
using HealthApp.Helpers;

namespace HealthApp.Droid
{
    [Activity(Label = "CustomUrlSchemeInterceptorActivity", NoHistory = true, LaunchMode = LaunchMode.SingleTask)]
    [IntentFilter(new[] { Intent.ActionView }, Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable }, DataSchemes = new[] { Constants.GoogleAndroidRedirectUriDataSchema }, DataPath = "/oauth2redirect")]
    public class CustomUrlSchemeInterceptorActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var uri = new Uri(Intent.Data.ToString());

            AuthenticationState.Authenticator.OnPageLoading(uri);

            var intent = new Intent(this, typeof(MainActivity));

            _ = intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.SingleTop);

            StartActivity(intent);

            Finish();
        }
    }
}