using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HealthApp.Common;
using HealthApp.Handlers;
using HealthApp.ViewModels;
using Xamarin.Auth;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HealthApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        //private OAuthGoogleLogInHandler _oAuthGoogleLogInHandler;
        private LoginViewModel _bindingContext = null;

        public LoginPage()
        {
            InitializeComponent();

            BindingContext = App.ViewModelLocator.LoginVm;
            _bindingContext = BindingContext as LoginViewModel;
        }

        private void GoogleAuthorizationTapped(object sender, System.EventArgs e)
        {
            _bindingContext?.GoogleAuthorizationCommand.Execute(null);
        }

        //[Obsolete]
        //private void GoogleAuthorizationTapped(object sender, System.EventArgs e)
        //{
        //    _oAuthGoogleLogInHandler = new OAuthGoogleLogInHandler();

        //    _oAuthGoogleLogInHandler.OAuthLogInResponseSuccessEvent += OAuthLogInResponseSuccess;
        //    _oAuthGoogleLogInHandler.OAuthLogInResponseErrorEvent += OAuthLogInResponseError;

        //    _oAuthGoogleLogInHandler.HandleOAuthLogIn();
        //}

        //[Obsolete]
        //async void OAuthLogInResponseSuccess(object sender, OAuthLogInResponseEventArgs e)
        //{
        //    if (_oAuthGoogleLogInHandler != null)
        //    {
        //        _oAuthGoogleLogInHandler.OAuthLogInResponseSuccessEvent -= OAuthLogInResponseSuccess;
        //        _oAuthGoogleLogInHandler.OAuthLogInResponseErrorEvent -= OAuthLogInResponseError;
        //    }

        //    AccountStore store = AccountStore.Create();

        //    IEnumerable<Account> account = store.FindAccountsForService(Constants.AppName);

        //    await FetchUserDataAsync(account.ElementAt(0));
        //}

        //void OAuthLogInResponseError(object sender, OAuthLogInResponseEventArgs e)
        //{
        //    Console.WriteLine("OAuthLogInResponseError");
        //}

        //private async Task FetchUserDataAsync(Account account)
        //{
        //    var request = new OAuth2Request("GET", new Uri(Constants.GoogleUserInfoUrl), null, account);
        //    var response = await request.GetResponseAsync();

        //    if (response != null)
        //    {
        //        string userJson = await response.GetResponseTextAsync();
        //    }
        //}
    }
}