using System;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HealthApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewsPage : ContentPage
    {
        public NewsPage()
        {
            InitializeComponent();

            Shell.SetNavBarIsVisible(this, false);
            Shell.SetTabBarIsVisible(this, false);
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new Components.PopupComponents.NewsPopup(vm.SelectedRecord));
        }
    }
}