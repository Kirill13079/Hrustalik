using HealthApp.AppSettings;
using HealthApp.Extensions;
using HealthApp.Interfaces;
using HealthApp.Styles;
using HealthApp.Utils;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HealthApp.Helpers
{
    public static class ThemeHelper
    {
        public static void GetAppTheme()
        {
            string theme = Settings.GetSetting(prefrence: Settings.AppPrefrences.AppTheme);

            if (theme != null)
            {
                ThemeEnum.Theme appTheme = theme.ConvertToEnum<ThemeEnum.Theme>();

                switch (appTheme)
                {
                    case ThemeEnum.Theme.LightTheme:
                        ChangeToLightTheme();
                        break;
                    case ThemeEnum.Theme.DarkTheme:
                        ChangeToDarkTheme();
                        break;
                    case ThemeEnum.Theme.SystemPreferred:
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

            Settings.AddSetting(prefrence: Settings.AppPrefrences.AppTheme, setting: EnumExtension.ConvertToString(ThemeEnum.Theme.LightTheme));
        }

        public static void ChangeToDarkTheme()
        {
            Application.Current.Resources = new DarkTheme();

            DependencyService.Get<IStatusBar>().ChangeStatusBarColorToBlack();

            Settings.AddSetting(prefrence: Settings.AppPrefrences.AppTheme, setting: EnumExtension.ConvertToString(ThemeEnum.Theme.DarkTheme));
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

            Settings.AddSetting(prefrence: Settings.AppPrefrences.AppTheme, setting: EnumExtension.ConvertToString(ThemeEnum.Theme.SystemPreferred));
        }
    }
}
