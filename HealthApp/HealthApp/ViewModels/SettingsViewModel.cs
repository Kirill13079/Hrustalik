using HealthApp.AppSettings;
using HealthApp.Common.Model;
using HealthApp.Common.Model.Helper;
using HealthApp.Helpers;
using HealthApp.Service;
using MvvmHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HealthApp.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private static readonly SettingsViewModel _instance = new SettingsViewModel();

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

        private bool _isLightTheme;
        public bool IsLightTheme
        {
            get { return _isLightTheme; }
            set
            {
                _isLightTheme = value;
                OnPropertyChanged();

                if (IsLightTheme)
                {
                    IsDarkTheme = false;
                    IsSystemPreferredTheme = false;
                }
            }
        }

        private bool _isDarkTheme;
        public bool IsDarkTheme
        {
            get { return _isDarkTheme; }
            set
            {
                _isDarkTheme = value;
                OnPropertyChanged();

                if (IsDarkTheme)
                {
                    IsLightTheme = false;
                    IsSystemPreferredTheme = false;
                }
            }
        }

        private bool _isSystemPreferredTheme;
        public bool IsSystemPreferredTheme
        {
            get { return _isSystemPreferredTheme; }
            set
            {
                _isSystemPreferredTheme = value;
                OnPropertyChanged();

                if (IsSystemPreferredTheme)
                {
                    IsLightTheme = false;
                    IsDarkTheme = false;
                }
            }
        }

        public ICommand ThemeChangeCommand => new Command((obj) =>
        {
            var appTheme = EnumsHelper.ConvertToEnum<Settings.Theme>((string)obj);

            switch (appTheme)
            {
                case Settings.Theme.LightTheme:
                    IsLightTheme = true;
                    ThemeHelper.ChangeToLightTheme();
                    break;
                case Settings.Theme.DarkTheme:
                    IsDarkTheme = true;
                    ThemeHelper.ChangeToDarkTheme();
                    break;
                case Settings.Theme.SystemPreferred:
                    IsSystemPreferredTheme = true;
                    ThemeHelper.ChangeToSystemPreferredTheme();
                    break;
                default:
                    IsSystemPreferredTheme = true;
                    ThemeHelper.ChangeToSystemPreferredTheme();
                    break;
            }
        });

        public static SettingsViewModel Instance => _instance;

        public SettingsViewModel()
        {
            Customer = new Customer();

            _ = GetSettings();
        }

        private async Task GetSettings()
        {
            await GetCustomer().ConfigureAwait(false);
            GetTheme();
        }

        private async Task GetCustomer()
        {
            if (Preferences.Get("ExistingUser", false))
            {
                string url = ApiRoutes.BaseUrl + ApiRoutes.GetCustomer;

                var response = await ApiCaller.Get(url);

                if (!string.IsNullOrWhiteSpace(response))
                { 
                    var customer = JsonConvert.DeserializeObject<Customer>(response);

                    Customer = customer;

                    IsLoggedIn = true;
                }
            }
            else
            {
                IsLoggedIn = false;
            }
        }

        private void GetTheme()
        {
            string theme = Settings.GetSetting(Settings.AppPrefrences.AppTheme);

            var appTheme = EnumsHelper.ConvertToEnum<Settings.Theme>(theme);

            switch (appTheme)
            {
                case Settings.Theme.LightTheme:
                    IsLightTheme = true;
                    break;
                case Settings.Theme.DarkTheme:
                    IsDarkTheme = true;
                    break;
                case Settings.Theme.SystemPreferred:
                    IsSystemPreferredTheme = true;
                    break;
                default:
                    IsSystemPreferredTheme = true;
                    break;
            }
        }
    }
}
