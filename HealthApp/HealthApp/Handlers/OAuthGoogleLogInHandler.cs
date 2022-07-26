using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HealthApp.Common;
using HealthApp.Helpers;
using Xamarin.Auth;

namespace HealthApp.Handlers
{
    public class OAuthLogInResponseEventArgs : EventArgs
    {
        public OAuthLogInResponseEventArgs()
        {

        }
    }

    public class OAuthGoogleLogInHandler
    {
        private Account _account;

        [Obsolete]
        private readonly AccountStore _store;

        public event EventHandler<OAuthLogInResponseEventArgs> OAuthLogInResponseSuccessEvent;
        public event EventHandler<OAuthLogInResponseEventArgs> OAuthLogInResponseErrorEvent;

        [Obsolete]
        public OAuthGoogleLogInHandler()
        {
            _store = AccountStore.Create();
        }

        [Obsolete]
        internal void HandleOAuthLogIn()
        {
            string clientId = Constants.GoogleAndroidClientId;
            string redirectUri = Constants.GoogleAndroidRedirectUri;

            _account = _store.FindAccountsForService(Constants.AppName).FirstOrDefault();

            var authenticator = new OAuth2Authenticator(
                clientId,
                null,
                Constants.GoogleScope,
                new Uri(Constants.GoogleAuthorizeUrl),
                new Uri(redirectUri),
                new Uri(Constants.GoogleAccessTokenUrl),
                null,
                true)
            {
                AllowCancel = true,
                Title = "Вход через Google"
            };

            authenticator.Completed += OnAuthCompleted;
            authenticator.Error += OnAuthError;

            AuthenticationStateHelper.Authenticator = authenticator;

            var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
            presenter.Login(authenticator);
        }

        [Obsolete]
        async void OnAuthCompleted(object sender, AuthenticatorCompletedEventArgs e)
        {
            var authenticator = sender as OAuth2Authenticator;

            if (authenticator != null)
            {
                authenticator.Completed -= OnAuthCompleted;
                authenticator.Error -= OnAuthError;
            }

            if (e.IsAuthenticated)
            {
                if (_account != null)
                {
                    _store.Delete(_account, Constants.AppName);
                }

                await _store.SaveAsync(_account = e.Account, Constants.AppName);

                OAuthLogInResponseSuccessEvent?.Invoke(this, new OAuthLogInResponseEventArgs());
            }
        }

        [Obsolete]
        void OnAuthError(object sender, AuthenticatorErrorEventArgs e)
        {
            var authenticator = sender as OAuth2Authenticator;

            if (authenticator != null)
            {
                authenticator.Completed -= OnAuthCompleted;
                authenticator.Error -= OnAuthError;
            }

            OAuthLogInResponseErrorEvent?.Invoke(this, new OAuthLogInResponseEventArgs());
        }
    }
}
