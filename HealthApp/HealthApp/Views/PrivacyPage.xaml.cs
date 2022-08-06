using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HealthApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PrivacyPage 
    {
        public PrivacyPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (Shell.Current.Height > 0)
            {
                content.HeightRequest = Shell.Current.Height;
            }
        }

        private async void OnSwipeDownToClosePopupPageCloseAction()
        {
            await Service.NavigationService.NavigateRemovePopupPageAsync(this);
        }
    }
}