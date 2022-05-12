using HealthApp.Service;
using HealthApp.Views;
using HealthApp.Views.Authorization;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: ExportFont("FontAwesome-Regular.ttf", Alias = "AwesomeRegular")]
[assembly: ExportFont("FontAwesome-Solid.ttf", Alias = "AwesomeSolid")]
[assembly: ExportFont("Gilroy-ExtraBold.otf", Alias = "ExtraBold")]
[assembly: ExportFont("Gilroy-Light.otf", Alias = "Light")]
[assembly: ExportFont("gilroy-regular.ttf", Alias = "Regular")]
namespace HealthApp
{
    public partial class App : Application
    {
        public Util Util { get; set; }

        public App()
        {
            InitializeComponent();

            Util = new Util();

            if (Preferences.Get("ExistingUser", false))
            {
                MainPage = new AppShell();
            }
            else 
            {
                MainPage = new NavigationPage(new AnonymousPage());
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
