using HealthApp.ViewModels;
using HealthApp.ViewModels.Data;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HealthApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        private readonly SettingsViewModel _bindingContext;
        private AppThemeViewModel _currentAppTheme = new AppThemeViewModel();

        public SettingsPage()
        {
            InitializeComponent();

            BindingContext = App.ViewModelLocator.SettingsVM;

            _bindingContext = BindingContext as SettingsViewModel;
            _currentAppTheme = _bindingContext.AppThemeItems.FirstOrDefault(theme => theme.IsActive);
        }

        private void AppThemeTapped(object sender, System.EventArgs e)
        {
            var frame = (Frame)sender;
            var appTheme = (AppThemeViewModel)frame.BindingContext;

            if (appTheme != null)
            {
                if (appTheme != _currentAppTheme)
                {
                    appTheme.IsActive = true;
                    _currentAppTheme.IsActive = false;

                    _currentAppTheme = appTheme;

                    _bindingContext.AppThemeChangedCommand.Execute(_currentAppTheme);
                }
            }
        }
    }
}