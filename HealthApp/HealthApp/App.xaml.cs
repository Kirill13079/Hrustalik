using HealthApp.Service;
using HealthApp.Views;
using HealthApp.Views.Authorization;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: ExportFont("FontAwesome-Regular.ttf", Alias = "AwesomeRegular")]
[assembly: ExportFont("FontAwesome-Solid.ttf", Alias = "AwesomeSolid")]
[assembly: ExportFont("Gilroy-Black.ttf", Alias = "Black")]
[assembly: ExportFont("Gilroy-Bold.ttf", Alias = "Bold")]
[assembly: ExportFont("Gilroy-Extrabold.ttf", Alias = "ExtraBold")]
[assembly: ExportFont("GGilroy-Light.ttf", Alias = "Light")]
[assembly: ExportFont("Gilroy-Regular.ttf", Alias = "Regular")]
[assembly: ExportFont("Gilroy-Medium.ttf", Alias = "Medium")]
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
