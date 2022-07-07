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
using System.Net.Http;
using HealthApp.Models;

namespace HealthApp.ViewModels
{
    public class LoginViewModel : ViewBaseModel
    {
        private string _email = "kirill.zhenkevich13@gmail.com";
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        private string _password = "qwaserdf13QQ??";
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
            AuthorizationCommand = new Command(async () => await AuthorizationCommandHadlerAsync());
            GoogleAuthorizationCommand = new Command(async () => await GoogleAuthorizationCommandHandlerAsync("Google"));
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
                            string url = ApiRoutes.BaseUrl + ApiRoutes.Login;

                            var userInfo = new Login
                            {
                                Email = googleResponse.Email,
                                Password = authToken,
                                AccessToken = true
                            };

                            var response = await ApiCaller.Post(url, userInfo);

                            if (!string.IsNullOrWhiteSpace(response))
                            {
                                var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(response);

                                App.ViewModelLocator.SettingsVM.IsLoggedIn = true;
                                App.ViewModelLocator.SettingsVM.Customer = loginResponse.Customer;

                                Settings.AddSetting(Settings.AppPrefrences.token, loginResponse.Token);

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
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Вход", $"Во время входа в систему произошла ошибка {ex.Message}", "Понятно");
            }
        }

        private async Task AuthorizationCommandHadlerAsync()
        {
            string url = ApiRoutes.BaseUrl + ApiRoutes.Login;

            var userInfo = new Login
            {
                Email = Email.Trim(),
                Password = Password
            };

            var response = await ApiCaller.Post(url, userInfo);

            if (!string.IsNullOrWhiteSpace(response))
            {
                DialogsHelper.ProgressDialog.Show();

                var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(response);

                App.ViewModelLocator.SettingsVM.IsLoggedIn = true;
                App.ViewModelLocator.SettingsVM.Customer = loginResponse.Customer;

                Settings.AddSetting(Settings.AppPrefrences.token, loginResponse.Token);

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

                DialogsHelper.ProgressDialog.Hide();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Вход", "Во время входа в систему произошла ошибка", "Понятно");
            }
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
