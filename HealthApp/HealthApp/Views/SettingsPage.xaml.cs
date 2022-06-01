using HealthApp.Models;
using HealthApp.ViewModels;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HealthApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();

            BindingContext = ViewModels.SettingsViewModel.Instance;

            var bindingContext = BindingContext as ViewModels.SettingsViewModel;

            string currentThemeTitle = bindingContext.GetTheme();

            SetCurrentThemeCheckbox(bindingContext, currentThemeTitle);

        }

        private void Theme_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            var checkBox = (CheckBox)sender;

            var themeModel = (ThemeModel)checkBox.BindingContext;

            if (themeModel.IsActive)
            {
                var bindingContext = BindingContext as ViewModels.SettingsViewModel;

                ResetСheckboxState(bindingContext, themeModel);

                bindingContext.ThemeChangeCommand.Execute(themeModel);
            }
        }

        private void ResetСheckboxState(
            SettingsViewModel bindingContext,
            ThemeModel activeTheme)
        {
            foreach (var themeModel in bindingContext.ThemeItems)
            {
                if (themeModel != activeTheme)
                {
                    themeModel.IsActive = false;
                }
            }
        }

        private void SetCurrentThemeCheckbox(
            SettingsViewModel bindingContext, 
            string currentThemeTitle)
        {
            var currentThemeModel = bindingContext.ThemeItems
                .Where(x => x.Title == currentThemeTitle)
                .FirstOrDefault();

            foreach (var themeModel in bindingContext.ThemeItems)
            {
                if (themeModel == currentThemeModel)
                {
                    themeModel.IsActive = true;
                }
            }
        }

    }
}