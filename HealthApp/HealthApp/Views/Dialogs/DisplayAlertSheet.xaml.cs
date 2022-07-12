using System;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms.Xaml;

namespace HealthApp.Views.Dialogs
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DisplayAlertSheet : PopupPage
    {
        private Func<bool, Task> _callback;

        public DisplayAlertSheet(string title, string message, string accept, string cancel, Func<bool, Task> callback)
        {
            InitializeComponent();

            titleLabel.Text = title;
            messageLabel.Text = message;
            acceptLabel.Text = accept;
            cancelLabel.Text = cancel;

            _callback = callback;
        }

        private async void OnAcceptTapped(object sender, EventArgs e)
        {
            await _callback.Invoke(true);
        }

        private async void OnCloseTapped(object sender, EventArgs e)
        {
            await _callback.Invoke(false);
        }
    }
}