using HealthApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HealthApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WebNewsPage : ContentPage
    {
        private RecordModel _record = null;

        public WebNewsPage(RecordModel record)
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

        private async void WebView_Navigated(object sender, WebNavigatedEventArgs e)
        {
            await progressBar.ProgressTo(1, 100, Easing.Linear);

            progressBar.IsVisible = false;
        }

        private async void WebView_Navigating(object sender, WebNavigatingEventArgs e)
        {
            await progressBar.ProgressTo(0.95, 7000, Easing.Linear);
        }
    }
}