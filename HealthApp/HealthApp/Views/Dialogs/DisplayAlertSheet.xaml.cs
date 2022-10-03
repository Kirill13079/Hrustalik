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
    public partial class DisplayAlertSheet : PopupPage
    {
        private static DisplayAlertSheet _instance;

        private Func<bool, Task> _callback;

        private DisplayAlertSheet()
        {
            InitializeComponent();

            BindingContext = this;
        }

        public static DisplayAlertSheet Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DisplayAlertSheet();
                }

                return _instance;
            }

            set => _instance = value;
        }

        public string Message { get; set; }

        public string Accept { get; set; }

        public string Cancel { get; set; }

        [Obsolete]
        public async Task ShowAsync(Func<bool, Task> callback)
        {
            if (!PopupNavigation.PopupStack.Contains(this))
            {
                _callback = callback;

                await Application.Current.MainPage.Navigation.PushPopupAsync(this);
            }
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