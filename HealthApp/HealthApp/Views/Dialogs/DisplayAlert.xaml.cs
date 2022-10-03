using System;
using System.Linq;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HealthApp.Views.Dialogs
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DisplayAlert : PopupPage
    {
        private static DisplayAlert _instance;

        private DisplayAlert()
        {
            InitializeComponent();

            BindingContext = this;
        }

        public static DisplayAlert Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DisplayAlert();
                }

                return _instance;
            }

            set => _instance = value;
        }

        public string Message { get; set; }

        [Obsolete]
        public async Task ShowAsync()
        {
            if (!PopupNavigation.PopupStack.Contains(this))
            {
                await Application.Current.MainPage.Navigation.PushPopupAsync(this);
            }
        }

        private async void OnCloseTappedAsync(object sender, EventArgs e)
        {
            await Application.Current.MainPage.Navigation.PopPopupAsync();
        }
    }
}