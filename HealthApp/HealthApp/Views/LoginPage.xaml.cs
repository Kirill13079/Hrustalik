using System;
using HealthApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HealthApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private readonly LoginViewModel _bindingContext = null;

        public LoginPage()
        {
            InitializeComponent();

            BindingContext = App.ViewModelLocator.LoginVm;
            _bindingContext = BindingContext as LoginViewModel;

            _bindingContext?.СhangeStateLoginPageCommand.Execute(false);

            Shell.SetNavBarIsVisible(this, false);
            Shell.SetTabBarIsVisible(this, false);
        }

        private void OnGoogleAuthorizationTapped(object sender, EventArgs e)
        {
            _bindingContext?.GoogleAuthorizationCommand.Execute(null);
        }

        private void OnRegisterLinkTapped(object sender, EventArgs e)
        {
            _bindingContext?.СhangeStateLoginPageCommand.Execute(true);
        }

        private void OnBackButtonTapped(object sender, EventArgs e)
        {
            _bindingContext?.GoBackPageCommand.Execute(null);
        }
    }
}