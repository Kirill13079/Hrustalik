using HealthApp.ViewModels.Data;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HealthApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WebPage
    {
        private readonly RecordViewModel _record;

        public WebPage(RecordViewModel record)
        {
            InitializeComponent();

            _record = record;

            title.Text = _record.Name;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            webView.Source = _record.Source;

            if (Shell.Current.Height > 0)
            {
                content.HeightRequest = Shell.Current.Height - 100;
                header.HeightRequest = 60;
                swiper.HeightRequest = 5;
                webView.HeightRequest = content.HeightRequest - (header.HeightRequest + swiper.HeightRequest);
            }
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

        private async void CloseWebTapped(object sender, System.EventArgs e)
        {
            await Service.NavigationService.NavigateRemovePopupPageAsync(this);
        }

        private async void SwipeDownToClosePopupPageCloseAction()
        {
            await Service.NavigationService.NavigateRemovePopupPageAsync(this);
        }
    }
}