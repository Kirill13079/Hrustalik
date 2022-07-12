using HealthApp.AppSettings;
using HealthApp.Common.Model.Helper;
using HealthApp.Common.Model.Request;
using HealthApp.Service;
using Xamarin.Forms;
using Newtonsoft.Json;
using HealthApp.Common.Model.Response;
using HealthApp.Helpers;
using Xamarin.Essentials;
using HealthApp.ViewModels.Base;
using System.Threading.Tasks;
using System.Windows.Input;
using System;
using HealthApp.Common;
using HealthApp.Models;
using Rg.Plugins.Popup.Extensions;
using HealthApp.Views.Dialogs;
using Rg.Plugins.Popup.Services;

namespace HealthApp.ViewModels
{
    public class LoginViewModel : ViewBaseModel
    {
        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public ICommand GoogleAuthorizationCommand { get; }

        public LoginViewModel()
        {
            AuthorizationCommand = new Command(async () =>
            {
                DialogsHelper.ProgressDialog.Show();

                await AuthorizationCommandHadlerAsync();

                DialogsHelper.ProgressDialog.Hide();
            });
            GoogleAuthorizationCommand = new Command(async () =>
            {
                DialogsHelper.ProgressDialog.Show();

                await GoogleAuthorizationCommandHandlerAsync("Google");

                DialogsHelper.ProgressDialog.Hide();
            });
        }

        private async Task GoogleAuthorizationCommandHandlerAsync(string scheme)
        {
            try
            {
                WebAuthenticatorResult result = null;

                if (scheme.Equals("Google"))
                {
                    var authUrl = new Uri($"{ApiRoutes.BaseUrl}{ApiRoutes.MobileAuth}/{scheme}");
                    var callbackUrl = new Uri($"{Constants.CallbackDataSchema}://");

                    result = await WebAuthenticator.AuthenticateAsync(authUrl, callbackUrl);
                }

                if (result != null)
                {
                    string authToken = result?.AccessToken ?? result?.IdToken;

                    if (!string.IsNullOrWhiteSpace(authToken))
                    {
                        var googleResponse = await GetInfoGoogleUserAsync(authToken);

                        if (googleResponse != null)
                        {
                            var userInfo = new Login
                            {
                                Email = googleResponse.Email,
                                Password = authToken,
                                AccessToken = true
                            };

                            bool isLogin = await LoginUserAsync(userInfo);

                            if (!isLogin)
                            {
                                await AlertDialogService.ShowDialogAsync("Вход в систему", "Во время входа в систему произошла ошибка", "Понятно");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await AlertDialogService.ShowDialogAsync("Вход в систему", $"Во время входа в систему произошла ошибка: {ex.Message.ToLower()}", "Понятно");
            }
        }

        private async Task AuthorizationCommandHadlerAsync()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                await AlertDialogService.ShowDialogAsync("Вход в систему", "Для входа в систему необходимо заполнить все поля", "Понятно");

                return;
            }

            var userInfo = new Login
            {
                Email = Email.Trim(),
                Password = Password
            };

            bool isLogin = await LoginUserAsync(userInfo);

            if (!isLogin)
            {
                await AlertDialogService.ShowDialogAsync("Вход в систему", "Во время входа в систему произошла ошибка", "Понятно");
            }
        }

        private async Task<bool> LoginUserAsync(Login userInfo)
        {
            string url = ApiRoutes.BaseUrl + ApiRoutes.Login;

            if (userInfo != null)
            {
                var response = await ApiCaller.Post(url, userInfo);

                if (!string.IsNullOrWhiteSpace(response))
                {
                    var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(response);

                    App.ViewModelLocator.SettingsVM.IsLoggedIn = true;
                    App.ViewModelLocator.SettingsVM.Customer = loginResponse.Customer;

                    Settings.AddSetting(prefrence: Settings.AppPrefrences.token, setting: loginResponse.Token);

                    Task[] tasks =
                    {
                        App.ViewModelLocator.MainVm.GetDataAsync(),
                        App.ViewModelLocator.CategoryVm.GetDataAsync(),
                        App.ViewModelLocator.BookmarkVm.GetDataAsync()
                    };

                    await Task.WhenAll(tasks);

                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        Navigation.GoBackAsync();
                    });

                    return true;
                }
            }

            return false;
        }

        private async Task<GoogleResponseModel> GetInfoGoogleUserAsync(string authToken)
        {
            var response = await ApiCaller.GetTest(ApiRoutes.GoogleAuth + authToken);

            if (!string.IsNullOrWhiteSpace(response))
            {
                var googleUserJson = JsonConvert.DeserializeObject<GoogleResponseModel>(response);

                return googleUserJson;
            }

            return null;
        }
    }
}
