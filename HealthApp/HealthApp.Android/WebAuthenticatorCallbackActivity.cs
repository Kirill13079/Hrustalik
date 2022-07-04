using Android.App;
using Android.Content;
using Android.Content.PM;
using HealthApp.Common;

namespace HealthApp.Droid
{
    [Activity(NoHistory = true, LaunchMode = LaunchMode.SingleTop, Exported = true)]
    [IntentFilter(
        new[] { Intent.ActionView }, 
        Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable }, 
        DataScheme = Constants.CallbackDataSchema)]
    public class WebAuthenticatorCallbackActivity : Xamarin.Essentials.WebAuthenticatorCallbackActivity
    {

    }
}