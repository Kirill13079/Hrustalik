using HealthApp.AppSettings;
using HealthApp.Common.Model;
using HealthApp.Helpers;
using HealthApp.Service;
using HealthApp.ViewModels.Base;
using HealthApp.ViewModels.Data;
using MvvmHelpers;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HealthApp.ViewModels
{
    public class SettingsViewModel : ViewBaseModel
    {
        private Customer _customer = new Customer();
        public Customer Customer
        {
            get => _customer;
            set
            {
                _customer = value;
                OnPropertyChanged();
            }
        }

        private ObservableRangeCollection<AppThemeViewModel> _appThemeItems;
        public ObservableRangeCollection<AppThemeViewModel> AppThemeItems
        {
            get => _appThemeItems;
            set
            {
                _appThemeItems = value;
                OnPropertyChanged();
            }
        }

        private bool _isLoggedIn;
        public bool IsLoggedIn
        {
            get => _isLoggedIn;
            set
            {
                _isLoggedIn = value;
                OnPropertyChanged();
            }
        }

        public ICommand AppThemeChangedCommand { get; set; }

        public ICommand OpenAuthorsAndCategoriesPageCommand { get; set; }

        public SettingsViewModel()
        {
            LoginCommand = new Command(LoginCommandHadler);
            SignOutCommand = new Command(async () => await SignOutCommandHandlerAsync());
            OpenAuthorsAndCategoriesPageCommand = new Command(async() => await OpenAuthorsAndCategoriesPageCommandHadlerAsync());
            AppThemeChangedCommand = new Command<AppThemeViewModel>(theme => AppThemeChangedCommandHandler(theme));

            _ = GetSettingsAsync().ConfigureAwait(false);
        }

        private async Task GetSettingsAsync()
        {
            var customer = await ApiManagerService.GetCustomerAsync();;

            if (customer != null)
            {
                Customer = customer;

                IsLoggedIn = true;
            }
            else
            {
                IsLoggedIn = false;
            }

            SetAppThemeItems();

            string theme = Settings.GetSetting(prefrence: Settings.AppPrefrences.AppTheme);

            foreach (var appTheme in AppThemeItems)
            {
                appTheme.IsActive = appTheme.Title == theme;
            }
        }

        private void SetAppThemeItems()
        {
            ThemesHelper.GetAppTheme();

            AppThemeItems = new ObservableRangeCollection<AppThemeViewModel>()
            {
                {
                    new AppThemeViewModel
                    {
                        Title = "LightTheme",
                        Subtitle = "Светлая"
                    }
                },
                {
                    new AppThemeViewModel
                    {
                        Title = "DarkTheme",
                        Subtitle = "Темная"
                    }
                },
                {
                    new AppThemeViewModel
                    {
                        Title = "SystemPreferred",
                        Subtitle = "Системная"
                    }
                }
            };
        }

        private void AppThemeChangedCommandHandler(AppThemeViewModel theme)
        {
            var appTheme = EnumsHelper.ConvertToEnum<Settings.Theme>(theme);

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
        }

        private async Task SignOutCommandHandlerAsync()
        {
            bool action = await AlertDialogService.ShowDialogConfirmationAsync("Выйти?", "Вы уверены, что хотите выйти?", "Да", "Нет");

            if (action)
            {
                DialogsHelper.ProgressDialog.Show();

                Settings.RemoveSetting(prefrence: Settings.AppPrefrences.token);

                Task[] tasks =
                {
                    App.ViewModelLocator.MainVm.GetDataAsync(),
                    App.ViewModelLocator.CategoryVm.GetDataAsync(),
                    App.ViewModelLocator.BookmarkVm.GetDataAsync()
                };

                await Task.WhenAll(tasks);

                Customer = null;
                IsLoggedIn = false;

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Navigation.NavigateToAsync(route: "login", model: Customer);
                });

                DialogsHelper.ProgressDialog.Hide();
            }
        }

        private void LoginCommandHadler()
        {
            Navigation.NavigateToAsync(route: "login", model: Customer);
        }

        private async Task OpenAuthorsAndCategoriesPageCommandHadlerAsync()
        {
            DialogsHelper.ProgressDialog.Show();

            await Task.Delay(100); // show UI

            await Navigation.NavigateToAsync("authorsAndCategories");

            DialogsHelper.ProgressDialog.Hide();
        }
    }
}
