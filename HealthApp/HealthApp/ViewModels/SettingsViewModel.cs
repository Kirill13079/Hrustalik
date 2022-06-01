using HealthApp.AppSettings;
using HealthApp.Common.Model;
using HealthApp.Common.Model.Helper;
using HealthApp.Helpers;
using HealthApp.Models;
using HealthApp.Service;
using MvvmHelpers;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace HealthApp.ViewModels
{
    [QueryProperty("Parameter", "parameter")]
    public class SettingsViewModel : BaseViewModel
    {
        /// <summary>
        /// Получить экземпляр этого класса
        /// </summary>
        private static readonly SettingsViewModel _instance = new SettingsViewModel();
        public static SettingsViewModel Instance => _instance;

        /// <summary>
        /// Текущий пользователь
        /// </summary>
        private Customer _customer;
        public Customer Customer 
        { 
            get =>  _customer;
            set 
            {
                _customer = value; 
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Передаваемый параметр
        /// </summary>
        private string parameter;
        public string Parameter
        {
            get { return parameter; }
            set
            {
                if (value != null)
                {
                    parameter = Uri.UnescapeDataString(value);
                    Customer = JsonConvert.DeserializeObject<Customer>(parameter);
                    IsLoggedIn = true;
                }
            }
        }

        /// <summary>
        /// Коллекция доступных тем
        /// </summary>
        private ObservableRangeCollection<ThemeModel> _themeItems = new ObservableRangeCollection<ThemeModel>()
        {
            {
                new ThemeModel { Title = "LightTheme", Subtitle = "Светлая"}
            },
            {
                new ThemeModel { Title = "DarkTheme", Subtitle = "Темная"}
            },
            {
                new ThemeModel { Title = "SystemPreferred", Subtitle = "Системная"}
            }
        };
        public ObservableRangeCollection<ThemeModel> ThemeItems
        {
            get => _themeItems;
            set
            {
                _themeItems = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// True - если пользователь авторизован
        /// </summary>
        private bool _isLoggedIn;
        public bool IsLoggedIn
        {
            get { return _isLoggedIn; }
            set
            {
                _isLoggedIn = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Команда для смены темы
        /// </summary>
        public ICommand ThemeChangeCommand => new Command((obj) =>
        {
            var appTheme = EnumsHelper.ConvertToEnum<Settings.Theme>((ThemeModel)obj);

            switch (appTheme)
            {
                case Settings.Theme.LightTheme:
                    ThemeHelper.ChangeToLightTheme();
                    break;
                case Settings.Theme.DarkTheme:
                    ThemeHelper.ChangeToDarkTheme();
                    break;
                case Settings.Theme.SystemPreferred:
                    ThemeHelper.ChangeToSystemPreferredTheme();
                    break;
                default:
                    ThemeHelper.ChangeToSystemPreferredTheme();
                    break;
            }
        });

        /// <summary>
        /// Команда для выхода из под текущего пользователя
        /// </summary>
        public Command SignOutCommand => new Command(async () =>
        {
            bool action = await Application.Current.MainPage.DisplayAlert("Выйти?", "Вы уверены, что хотите выйти?", "Да", "Нет");

            if (action)
            {
                DialogsHelper.ProgressDialog.Show();

                Settings.ClearSecureSorage();
                Settings.RemoveSetting(Settings.AppPrefrences.token);

                Customer = null;
                IsLoggedIn = false;

                Navigation.NavigateTo("login", Customer);

                DialogsHelper.ProgressDialog.Hide();
            }
        });

        /// <summary>
        /// Команда перехода на форму входа
        /// </summary>
        public Command LogInPageCommand => new Command(() => 
        {
            Navigation.NavigateTo("login", Customer);
        });

        /// <summary>
        /// Конструктор
        /// </summary>
        public SettingsViewModel()
        {
            Customer = new Customer();

            _ = GetSettings();
        }

        /// <summary>
        /// Метод получающий текущую тему пользователя
        /// </summary>
        public string GetTheme()
        {
            string theme = Settings.GetSetting(Settings.AppPrefrences.AppTheme);

            var appTheme = EnumsHelper.ConvertToEnum<Settings.Theme>(theme);

            return EnumsHelper.ConvertToString(appTheme);
        }

        /// <summary>
        /// Метод получающий все настройки пользователя
        /// </summary>
        /// <returns></returns>
        private async Task GetSettings()
        {
            await GetCustomer().ConfigureAwait(false);
        }

        /// <summary>
        /// Метод получающий текущего пользователя
        /// </summary>
        /// <returns></returns>
        private async Task GetCustomer()
        {
            string token = Settings.GetSetting(Settings.AppPrefrences.token);

            if (!string.IsNullOrWhiteSpace(token))
            {
                string url = ApiRoutes.BaseUrl + ApiRoutes.GetCustomer;

                var response = await ApiCaller.Get(url);

                if (!string.IsNullOrWhiteSpace(response))
                {
                    var customer = JsonConvert.DeserializeObject<Customer>(response);

                    Customer = customer;

                    IsLoggedIn = true;
                }
                else 
                {
                    IsLoggedIn = false;

                    Settings.ClearSecureSorage();
                    Settings.RemoveSetting(Settings.AppPrefrences.token);
                }
            }
            else
            {
                IsLoggedIn = false;
            }
        }
    }
}
