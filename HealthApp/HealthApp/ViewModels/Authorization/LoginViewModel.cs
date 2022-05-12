using HealthApp.Common.Model.Helper;
using HealthApp.Common.Model.Request;
using HealthApp.Common.Model.Response;
using HealthApp.Service;
using HealthApp.Views;
using HealthApp.Views.Authorization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HealthApp.ViewModels.Authorization
{
    public class LoginViewModel : BaseViewModel
    {
        private string _email;

        public string Email 
        { 
            get => _email; 
            set => _email = value; 
        }

        private string _password;

        public string Password
        {
            get => _password; 
            set => _password = value; 
        }

        private string _confirmPassword;

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => _confirmPassword = value;
        }

        public ICommand GoToAnonymousPageCommand => new Command(async () => await GoToAnonymousPageCommandHandlerAsync());
        
        public ICommand GoToRegisterPageCommand => new Command(async () => await GoToRegisterPageCommandHandlerAsync());

        public ICommand GoToLoginPageCommand => new Command(async () => await GoToLoginPageCommandHandlerAsync());

        public ICommand GoToBackPageCommand => new Command(async () => await GoToBackPageCommandHandler());

        public ICommand RegisterCommand => new Command(async () => await RegisterCommandHandlerAsync());

        public ICommand LoginCommand => new Command(async () => await LoginCommandHandler());


        private async Task GoToBackPageCommandHandler()
        {
            await AppInstance.MainPage.Navigation.PopAsync();
        }

        private async Task GoToAnonymousPageCommandHandlerAsync()
        {
            await AppInstance.MainPage.Navigation.PushAsync(new AnonymousPage());
        }

        private async Task GoToLoginPageCommandHandlerAsync()
        {
            await AppInstance.MainPage.Navigation.PushAsync(new LoginPage());
        }

        private async Task GoToRegisterPageCommandHandlerAsync()
        {
            await AppInstance.MainPage.Navigation.PushAsync(new RegisterPage());
        }

        private async Task LoginCommandHandler()
        {
            string url = BaseUrl + ApiRoutes.Login;

            var userInfo = new Login
            {
                Email = Email.Trim(),
                Password = Password
            };

            var response = await ApiCaller.Post(url, userInfo);

            if (!string.IsNullOrWhiteSpace(response))
            {
                var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(response);

                AppInstance.Util.Customer = loginResponse.Customer;

                Preferences.Set("token", loginResponse.Token);
                Preferences.Set("ExistingUser", true);

                AppInstance.MainPage = new AppShell();
            }
            else
            {
                await AppInstance.MainPage.DisplayAlert("Вход", "Во время входа в систему произошла ошибка", "Понятно"); 
            }
        }

        private async Task RegisterCommandHandlerAsync()
        {
            if (Password != ConfirmPassword)
            { 
                await AppInstance.MainPage.DisplayAlert("Регистрация", "Пароли не совпадают", "Понятно");

                return;
            }

            string url = BaseUrl + ApiRoutes.Register;

            var userInfo = new Login
            {
                Email = Email.Trim(),
                Password = Password
            };

            var response = await ApiCaller.Post(url, userInfo);

            if (!string.IsNullOrWhiteSpace(response))
            {
                await AppInstance.MainPage.DisplayAlert("Регистрация", "Ваша регистрация прошла успешно", "Понятно");
                await AppInstance.MainPage.Navigation.PushAsync(new LoginPage());
            }
            else
            {
                await AppInstance.MainPage.DisplayAlert("Регистрация", "Во время регистрации произошла ошибка", "Понятно"); 
            }
        }
    }
}
