using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using FFImageLoading.Forms.Platform;
using PanCardView.Droid;
using Acr.UserDialogs;
using Rg.Plugins.Popup.Services;

namespace HealthApp.Droid
{
    [Activity(Label = "Hrustalik", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            Xamarin.Forms.Forms.Init(this, savedInstanceState);

            CachedImageRenderer.Init(true);

            _ = Firebase.FirebaseApp.InitializeApp(Application.ApplicationContext);

            Xamarin.Auth.Presenters.XamarinAndroid.AuthenticationConfiguration.Init(this, savedInstanceState);
            Xamarin.Auth.CustomTabsConfiguration.CustomTabsClosingMessage = null;

            Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, savedInstanceState);

            CardsViewRenderer.Preserve();

            UserDialogs.Init(() => this);

            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public async override void OnBackPressed()
        {
            if (Rg.Plugins.Popup.Popup.SendBackPressed(base.OnBackPressed))
            {
                await PopupNavigation.Instance.PopAsync();
            }
        }
    }
}