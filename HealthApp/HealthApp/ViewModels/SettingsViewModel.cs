using HealthApp.AppSettings;
using HealthApp.Common.Model;
using HealthApp.Helpers;
using HealthApp.Interfaces;
using HealthApp.Service;
using HealthApp.ViewModels.Data;
using MvvmHelpers;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HealthApp.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private readonly IApiManager _apiManager = new ApiManager();

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

        private ObservableRangeCollection<AppThemeViewModel> _themeItems = new ObservableRangeCollection<AppThemeViewModel>()
        {
            {
                new AppThemeViewModel { Title = "LightTheme", Subtitle = "Светлая"}
            },
            {
                new AppThemeViewModel { Title = "DarkTheme", Subtitle = "Темная"}
            },
            {
                new AppThemeViewModel { Title = "SystemPreferred", Subtitle = "Системная"}
            }
        };
        public ObservableRangeCollection<AppThemeViewModel> ThemeItems
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
            var appTheme = EnumsHelper.ConvertToEnum<Settings.Theme>((AppThemeViewModel)obj);

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

                Settings.RemoveSetting(Settings.AppPrefrences.token);

                await App.ViewModelLocator.MainVm.GetDataAsync();
                await App.ViewModelLocator.CategoryVm.GetDataAsync();
                await App.ViewModelLocator.BookmarkVm.GetDataAsync();

                Customer = null;
                IsLoggedIn = false;

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Navigation.NavigateTo("login", Customer);
                });

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

            _ = GetSettingsAsync().ConfigureAwait(false);
        }

        public string GetTheme()
        {
            string theme = Settings.GetSetting(Settings.AppPrefrences.AppTheme);
            var appTheme = EnumsHelper.ConvertToEnum<Settings.Theme>(theme);

            return EnumsHelper.ConvertToString(eff: appTheme);
        }

        private async Task GetSettingsAsync()
        {
            var customer = await _apiManager.GetCustomerAsync();

            if (customer != null)
            {
                Customer = customer;
                IsLoggedIn = true;
            }
            else
            {
                IsLoggedIn = false;
            }
        }
    }
}
