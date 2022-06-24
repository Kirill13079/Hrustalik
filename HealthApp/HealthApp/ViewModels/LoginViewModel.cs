using HealthApp.AppSettings;
using HealthApp.Common.Model.Helper;
using HealthApp.Common.Model.Request;
using HealthApp.Service;
using MvvmHelpers;
using System.Windows.Input;
using Xamarin.Forms;
using Newtonsoft.Json;
using HealthApp.Common.Model;
using System;
using HealthApp.Common.Model.Response;
using HealthApp.ViewModels.Main;
using HealthApp.Helpers;

namespace HealthApp.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private static readonly LoginViewModel _instance = new LoginViewModel();
        public static LoginViewModel Instance => _instance;

        private Customer _customer;
        public Customer Customer
        {
            get => _customer;
            set
            {
                _customer = value;
                OnPropertyChanged();
            }
        }

        private string _email = "kirill.zhenkevich13@gmail.com";
        public string Email
        {
            get => _email;
            set => _email = value;
        }

        private string _password = "qwaserdf13QQ??";
        public string Password
        {
            get => _password;
            set => _password = value;
        }

        public ICommand LogInCommand => new Command(async () => 
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

                SettingsViewModel.Instance.Customer = loginResponse.Customer;
                SettingsViewModel.Instance.IsLoggedIn = true;

                Settings.AddSetting(Settings.AppPrefrences.token, loginResponse.Token);

                await MainViewModel.Instance.GetDataAsync();

                Navigation.GoBack();

                DialogsHelper.ProgressDialog.Hide();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Вход", "Во время входа в систему произошла ошибка", "Понятно");
            }
        });
    }
}
