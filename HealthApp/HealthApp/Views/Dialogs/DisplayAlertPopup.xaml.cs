using System;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms.Xaml;

namespace HealthApp.Views.Dialogs
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DisplayAlertPopup : PopupPage
    {
        public DisplayAlertPopup(string title, string message, string accept, string cancel)
        {
            InitializeComponent();

            titleLabel.Text = title;
            messageLabel.Text = message;
            acceptLabel.Text = accept;
        }

        private void OnCloseTapped(object sender, EventArgs e)
        {
            _ = PopupNavigation.Instance.PopAsync();
        }
    }
}