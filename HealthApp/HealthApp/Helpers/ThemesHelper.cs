using HealthApp.AppSettings;
using HealthApp.Interfaces;
using HealthApp.Styles;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HealthApp.Helpers
{
    public static class ThemesHelper
    {
        public static void GetAppTheme()
        {
            string theme = Settings.GetSetting(prefrence: Settings.AppPrefrences.AppTheme);

            if (theme != null)
            {
                var appTheme = EnumsHelper.ConvertToEnum<Settings.Theme>(theme);

                switch (appTheme)
                {
                    case Settings.Theme.LightTheme:
                        ChangeToLightTheme();
                        break;
                    case Settings.Theme.DarkTheme:
                        ChangeToDarkTheme();
                        break;
                    case Settings.Theme.SystemPreferred:
                        ChangeToSystemPreferredTheme();
                        break;
                    default:
                        ChangeToSystemPreferredTheme();
                        break;
                }
            }
            else
            {
                ChangeToSystemPreferredTheme();
            }
        }

        public static void ChangeToLightTheme()
        {
            Application.Current.Resources = new LightTheme();

            DependencyService.Get<IStatusBar>().ChangeStatusBarColorToWhite();

            Settings.AddSetting(
                prefrence: Settings.AppPrefrences.AppTheme, 
                setting: EnumsHelper.ConvertToString(eff: Settings.Theme.LightTheme));
        }

        public static void ChangeToDarkTheme()
        {
            Application.Current.Resources = new DarkTheme();

            DependencyService.Get<IStatusBar>().ChangeStatusBarColorToBlack();

            Settings.AddSetting(
                prefrence: Settings.AppPrefrences.AppTheme, 
                setting: EnumsHelper.ConvertToString(eff: Settings.Theme.DarkTheme));
        }

        public static void ChangeToSystemPreferredTheme()
        {
            AppTheme appTheme = AppInfo.RequestedTheme;

            if (appTheme == AppTheme.Dark)
            {
                ChangeToDarkTheme();
            }
            else if (appTheme == AppTheme.Light)
            {
                ChangeToLightTheme();
            }
            else
            {
                ChangeToLightTheme();
            }

            Settings.AddSetting(
                prefrence: Settings.AppPrefrences.AppTheme, 
                setting: EnumsHelper.ConvertToString(eff: Settings.Theme.SystemPreferred));
        }
    }
}
