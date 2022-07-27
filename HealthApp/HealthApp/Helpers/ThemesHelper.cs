using HealthApp.AppSettings;
using HealthApp.Extensions;
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
                var appTheme = theme.ConvertToEnum<Settings.Theme>();

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

            Settings.AddSetting(prefrence: Settings.AppPrefrences.AppTheme, setting: EnumExtension.ConvertToString(Settings.Theme.LightTheme));
        }

        public static void ChangeToDarkTheme()
        {
            Application.Current.Resources = new DarkTheme();

            DependencyService.Get<IStatusBar>().ChangeStatusBarColorToBlack();

            Settings.AddSetting(prefrence: Settings.AppPrefrences.AppTheme, setting: EnumExtension.ConvertToString(Settings.Theme.DarkTheme));
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
                ChangeToDarkTheme();
            }

            Settings.AddSetting(prefrence: Settings.AppPrefrences.AppTheme, setting: EnumExtension.ConvertToString(Settings.Theme.SystemPreferred));
        }
    }
}
