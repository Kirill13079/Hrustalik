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
using System.ComponentModel.DataAnnotations;
using HealthApp.Models;

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

        private string _сonfirmedPassword;
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
                DialogsHelper.ProgressDialog.Show();

                await AuthorizationCommandHadlerAsync();

                DialogsHelper.ProgressDialog.Hide();
            });
            GoogleAuthorizationCommand = new Command(async () =>
            {
                DialogsHelper.ProgressDialog.Show();

                await GoogleAuthorizationCommandHandlerAsync();

                DialogsHelper.ProgressDialog.Hide();
            });
            RegistrationCommand = new Command(async () =>
            {
                DialogsHelper.ProgressDialog.Show();

                await RegistrationCommandHandlerAsync();

                DialogsHelper.ProgressDialog.Hide();
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
                    Uri authUrl = new Uri($"{ApiRoutes.BaseUrl}{ApiRoutes.MobileAuth}/{scheme}");
                    Uri callbackUrl = new Uri($"{Constants.CallbackDataSchema}://");

                    result = await WebAuthenticator.AuthenticateAsync(authUrl, callbackUrl);
                }

                if (result != null)
                {
                    string authToken = result?.AccessToken ?? result?.IdToken;

                    if (!string.IsNullOrWhiteSpace(authToken))
                    {
                        GoogleResponseModel googleResponse = await ApiManagerService.GetInfoGoogleUserAsync(authToken);

                        if (googleResponse != null)
                        {
                            Login userInfo = new Login
                            {
                                Email = googleResponse.Email,
                                Password = authToken,
                                AccessToken = true
                            };

                            bool isLogin = await AuthorizationUserAsync(userInfo, ApiRoutes.Login);

                            if (!isLogin)
                            {
                                await AlertDialogService.ShowDialogAsync(Title, "Во время входа в систему произошла ошибка", "Понятно");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await AlertDialogService.ShowDialogAsync(Title, $"Во время входа в систему произошла ошибка: {ex.Message.ToLower()}", "Понятно");
            }
        }

        private async Task AuthorizationCommandHadlerAsync()
        {
            bool isValidModel = await IsValidModelAsync();

            if (isValidModel)
            {
                Login userInfo = new Login
                {
                    Email = Email.Trim(),
                    Password = Password
                };

                bool isLogin = await AuthorizationUserAsync(userInfo, ApiRoutes.Login);

                if (!isLogin)
                {
                    await AlertDialogService.ShowDialogAsync(Title, "Во время входа в систему произошла ошибка", "Понятно");
                }
            }
        }

        private async Task RegistrationCommandHandlerAsync()
        {
            bool isValidModel = await IsValidModelAsync();

            if (isValidModel)
            {
                Login userInfo = new Login
                {
                    Email = Email.Trim(),
                    Password = Password
                };

                bool isRegistration = await AuthorizationUserAsync(userInfo, ApiRoutes.Register);

                if (!isRegistration)
                {
                    await AlertDialogService.ShowDialogAsync(Title, "Во время регистрации произошла ошибка", "Понятно");
                }
            }
        }

        private async Task<bool> AuthorizationUserAsync(Login userInfo, string route)
        {
            if (!string.IsNullOrWhiteSpace(route))
            {
                string url = ApiRoutes.BaseUrl + route;

                if (userInfo != null)
                {
                    string response = await ApiCallerService.Post(url, model: userInfo);

                    bool result = await LoginResponseAsync(response);

                    return result;
                }
            }

            return false;
        }

        private async Task<bool> LoginResponseAsync(string response)
        {
            if (!string.IsNullOrWhiteSpace(response))
            {
                LoginResponse loginResponse = JsonConvert.DeserializeObject<LoginResponse>(response);

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
                    NavigationService.GoBackAsync();
                });

                return true;
            }

            return false;
        }

        private async Task<bool> IsValidModelAsync()
        {
            if (IsRegistration)
            {
                if (string.IsNullOrWhiteSpace(Email)
                    || string.IsNullOrWhiteSpace(Password)
                    || string.IsNullOrWhiteSpace(ConfirmedPassword))
                {
                    await AlertDialogService.ShowDialogAsync(Title, "Для регистрации необходимо заполнить все поля", "Понятно");

                    return false;
                }

                if (Password != ConfirmedPassword)
                {
                    await AlertDialogService.ShowDialogAsync(Title, "Пароли не совпадают", "Понятно");

                    return false;
                }
            }
            else if (!IsRegistration)
            {
                if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
                {
                    await AlertDialogService.ShowDialogAsync(Title, "Для входа в систему необходимо заполнить все поля", "Понятно");

                    return false;
                }
            }

            bool isValidEmail = new EmailAddressAttribute().IsValid(Email);

            if (!isValidEmail)
            {
                await AlertDialogService.ShowDialogAsync(Title, $"{Email} - введный email не допустим", "Понятно");

                return false;
            }

            return true;
        }
    }
}