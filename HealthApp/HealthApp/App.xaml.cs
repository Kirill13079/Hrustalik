using HealthApp.Helpers;
using HealthApp.Service;
using Xamarin.Forms;

[assembly: ExportFont("MaterialIcons-Regular.ttf", Alias = "MaterialIcon")]
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
        private static ViewModelLocator _viewModelLocator;
        public static ViewModelLocator ViewModelLocator => _viewModelLocator ?? (_viewModelLocator = new ViewModelLocator());

        public App()
        {
            InitializeComponent();

            Device.SetFlags(new[] 
            { 
                "SwipeView_Experimental", 
                "CollectionView_Experimental" 
            });

            MainPage = new AppShell();
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
