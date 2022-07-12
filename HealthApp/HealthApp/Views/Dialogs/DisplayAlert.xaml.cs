using System;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HealthApp.Views.Dialogs
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DisplayAlert : PopupPage
    {
        public DisplayAlert(string title, string message, string accept)
        {
            InitializeComponent();

            titleLabel.Text = title;
            messageLabel.Text = message;
            acceptLabel.Text = accept;
        }

        private async void OnCloseTapped(object sender, EventArgs e)
        {
            await Application.Current.MainPage.Navigation.PopPopupAsync();
        }
    }
}