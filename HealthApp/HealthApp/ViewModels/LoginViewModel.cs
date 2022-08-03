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
using HealthApp.Utils;
using HealthApp.Extensions;
using System.Text.RegularExpressions;

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

        public ICommand GoogleAuthorizationCommand { get; }

        public ICommand СhangeStateLoginPageCommand { get; }

        public LoginViewModel()
        {
            AuthorizationCommand = new Command(async (route) =>
            {
                DialogsHelper.ProgressDialog.Show();

                switch (route)
                {
                    case "login":
                        await AuthorizationCommandHadlerAsync(ApiRoutes.Login);
                        break;
                    case "register":
                        await AuthorizationCommandHadlerAsync(ApiRoutes.Register);
                        break;
                    default:
                        break;
                }

                DialogsHelper.ProgressDialog.Hide();
            });
            GoogleAuthorizationCommand = new Command(async () =>
            {
                DialogsHelper.ProgressDialog.Show();

                await GoogleAuthorizationCommandHandlerAsync();

                DialogsHelper.ProgressDialog.Hide();
            });
            СhangeStateLoginPageCommand = new Command((isRegistration) => СhangeStateLoginPageCommandHandler((bool)isRegistration));
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
                                await AlertDialogService.ShowDialogAsync(
                                    title: Title,
                                    message: MessageEnum.Error.Login.DisplayName(),
                                    cancel: MessageEnum.Button.OK.DisplayName());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await AlertDialogService.ShowDialogAsync(
                    title: Title,
                    message: $"{MessageEnum.Error.Login.DisplayName()}: {ex.Message}",
                    cancel: MessageEnum.Button.OK.DisplayName());
            }
        }

        private void СhangeStateLoginPageCommandHandler(bool isRegistration)
        {
            IsRegistration = isRegistration;
            Title = IsRegistration ? Resources.Language.Resource.Register : Resources.Language.Resource.Login;
        }

        private async Task AuthorizationCommandHadlerAsync(string route)
        {
            string validModelMessage = ValidationModel();

            if (string.IsNullOrWhiteSpace(validModelMessage))
            {
                Login userInfo = new Login
                {
                    Email = Email.Trim(),
                    Password = Password
                };

                bool isLogin = await AuthorizationUserAsync(userInfo, route);

                if (!isLogin)
                {
                    await AlertDialogService.ShowDialogAsync(
                        title: Title,
                        message: MessageEnum.Error.Login.DisplayName(),
                        cancel: MessageEnum.Button.OK.DisplayName());
                }
            }
            else
            {
                await AlertDialogService.ShowDialogAsync(
                            title: Title,
                            message: validModelMessage,
                            cancel: MessageEnum.Button.OK.DisplayName());
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

        private string ValidationModel()
        {
            string result = string.Empty;

            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                result = MessageEnum.Error.EmptyEntry.DisplayName();

                return result;
            }

            if (IsRegistration)
            {
                if (string.IsNullOrWhiteSpace(ConfirmedPassword))
                {
                    result = MessageEnum.Error.EmptyEntry.DisplayName();

                    return result;
                }

                if (Password != ConfirmedPassword)
                {
                    result = MessageEnum.Error.ConfirmedPassword.DisplayName();

                    return result;
                }

                if (!Regex.IsMatch(Password, Constants.PasswordRegex))
                {
                    result = MessageEnum.Error.CorrectPassword.DisplayName();

                    return result;
                }
            }

            bool isValidEmail = Regex.IsMatch(
                input: Email.Trim(),
                pattern: Constants.EmailRegex,
                options: RegexOptions.IgnoreCase,
                matchTimeout: TimeSpan.FromMilliseconds(1));

            if (!isValidEmail)
            {
                result = MessageEnum.Error.CorrectEmail.DisplayName();

                return result;
            }

            return result;
        }
    }
}