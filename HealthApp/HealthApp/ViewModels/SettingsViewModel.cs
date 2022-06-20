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
    public class SettingsViewModel : BaseViewModel
    {
        private static SettingsViewModel _instance;
        public static SettingsViewModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SettingsViewModel();
                }

                return _instance;
            }
        }

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

        public ICommand ThemeChangeCommand => new Command((obj) =>
        {
            var appTheme = EnumsHelper.ConvertToEnum<Settings.Theme>((ThemeModel)obj);

            switch (appTheme)
            {
                case Settings.Theme.LightTheme:
                    ThemesHelper.ChangeToLightTheme();
                    break;
                case Settings.Theme.DarkTheme:
                    ThemesHelper.ChangeToDarkTheme();
                    break;
                case Settings.Theme.SystemPreferred:
                    ThemesHelper.ChangeToSystemPreferredTheme();
                    break;
                default:
                    ThemesHelper.ChangeToSystemPreferredTheme();
                    break;
            }
        });

        public Command SignOutCommand => new Command(async () =>
        {
            bool action = await Application.Current.MainPage.DisplayAlert("Выйти?", "Вы уверены, что хотите выйти?", "Да", "Нет");

            if (action)
            {
                DialogsHelper.ProgressDialog.Show();

                Settings.ClearSecureSorage();
                Settings.RemoveSetting(Settings.AppPrefrences.token);

                _ = BookmarksNewsViewModel.Instance.GetDataAsync().ConfigureAwait(false);

                Customer = null;
                IsLoggedIn = false;

                Navigation.NavigateTo("login", Customer);

                DialogsHelper.ProgressDialog.Hide();
            }
        });

        public Command LogInPageCommand => new Command(() => 
        {
            Navigation.NavigateTo("login", Customer);
        });

        public Command AuthorsAndCategoriesPageCommand => new Command(() =>
        {
            Navigation.NavigateTo("authorsAndCategories");
        });

        public SettingsViewModel()
        {
            Customer = new Customer();

            _ = GetSettings();
        }

        public string GetTheme()
        {
            string theme = Settings.GetSetting(Settings.AppPrefrences.AppTheme);

            var appTheme = EnumsHelper.ConvertToEnum<Settings.Theme>(theme);

            return EnumsHelper.ConvertToString(appTheme);
        }

        private async Task GetSettings()
        {
            await GetCustomer().ConfigureAwait(false);
        }

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
