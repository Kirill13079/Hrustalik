using HealthApp.ViewModels.Data;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HealthApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WebPage : ContentPage
    {
        private readonly RecordViewModel _record = null;

        public WebPage(RecordViewModel record)
        {
            InitializeComponent();

            Shell.SetTabBarIsVisible(this, false);

            _record = record;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            webView.Source = _record.Source;
        }

        private async void WebViewNavigated(object sender, WebNavigatedEventArgs e)
        {
            _ = await progressBar.ProgressTo(value: 1, length: 100, Easing.Linear);

            progressBar.IsVisible = false;
        }

        private async void WebViewNavigating(object sender, WebNavigatingEventArgs e)
        {
            _ = await progressBar.ProgressTo(value: 0.95, length: 7000, Easing.Linear);
        }
    }
}