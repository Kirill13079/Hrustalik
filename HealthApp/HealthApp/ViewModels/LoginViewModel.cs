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
using Acr.UserDialogs;
using System.Threading;

namespace HealthApp.ViewModels
{
    public class LoginViewModel : ViewBaseModel
    {
        private string _email = "dmitry.nagu51@gmail.com";
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        private string _password = "qwaserdf13QQ#";
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        private string _сonfirmedPassword = "qwaserdf13QQ#";
        public string ConfirmedPassword
        {
            get => _сonfirmedPassword;
            set
            {
                _сonfirmedPassword = value;
                OnPropertyChanged();
            }
        }

        private bool _isRegistration = false;
        public bool IsRegistration
        {
            get => _isRegistration;
            set
            {
                _isRegistration = value;
                OnPropertyChanged();
            }
        }

        public ICommand RegistrationCommand { get; }

        public ICommand GoogleAuthorizationCommand { get; }

        public LoginViewModel()
        {
            AuthorizationCommand = new Command(async () =>
            {
                //DialogsHelper.ProgressDialog.Show();
                var tokenSource = new CancellationTokenSource();

                var config = new ProgressDialogConfig()
                {
                    Title = "Авторизуемся",
                    CancelText = "Отмена",
                    IsDeterministic = false,
                    OnCancel = tokenSource.Cancel
                };

                var dialog = UserDialogs.Progress(config);

                using (dialog)
                {
                    dialog.Show();

                    await Task.Delay(10000);
                    await AuthorizationCommandHadlerAsync();
                }

                dialog.Hide();

                //DialogsHelper.ProgressDialog.Hide();
            });
            GoogleAuthorizationCommand = new Command(async () =>
            {
                UserDialogs.ShowLoading();

                await GoogleAuthorizationCommandHandlerAsync();

                UserDialogs.HideLoading();
            });
            RegistrationCommand = new Command(async () =>
            {
                UserDialogs.ShowLoading();

                await RegistrationCommandHandlerAsync();

                UserDialogs.HideLoading();
            });
        }

        private async Task GoogleAuthorizationCommandHandlerAsync()
        {
            try
            {
                string scheme = "Google";

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

                            bool isLogin = await AuthorizationUserAsync(userInfo, ApiRoutes.Login);

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

            bool isLogin = await AuthorizationUserAsync(userInfo, ApiRoutes.Login);

            if (!isLogin)
            {
                await AlertDialogService.ShowDialogAsync("Вход в систему", "Во время входа в систему произошла ошибка", "Понятно");
            }
        }

        private async Task RegistrationCommandHandlerAsync()
        {
            if (string.IsNullOrWhiteSpace(Email)
                || string.IsNullOrWhiteSpace(Password)
                || string.IsNullOrWhiteSpace(ConfirmedPassword))
            {
                await AlertDialogService.ShowDialogAsync("Регистрация", "Для регистрации необходимо заполнить все поля", "Понятно");

                return;
            }

            if (Password != ConfirmedPassword)
            {
                await AlertDialogService.ShowDialogAsync("Регистрация", "Пароли не совпадают", "Понятно");

                return;
            }

            var userInfo = new Login
            {
                Email = Email.Trim(),
                Password = Password
            };

            bool isRegistration = await AuthorizationUserAsync(userInfo, ApiRoutes.Register);

            if (!isRegistration)
            {
                await AlertDialogService.ShowDialogAsync("Регистрация", "Во время регистрации произошла ошибка", "Понятно");
            }
        }

        private async Task<bool> AuthorizationUserAsync(Login userInfo, string route)
        {
            if (!string.IsNullOrWhiteSpace(route))
            {
                string url = ApiRoutes.BaseUrl + route;

                if (userInfo != null)
                {
                    var response = await ApiCaller.Post(url, userInfo);
                    var result = await LoginResponse(response);

                    return result;
                }
            }

            return false;
        }

        private async Task<bool> LoginResponse(string response)
        {
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
