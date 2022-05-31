﻿using HealthApp.AppSettings;
using HealthApp.Helpers;
using HealthApp.Interfaces;
using HealthApp.Styles;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HealthApp.Themes
{
    public static class ThemeHelper
    {
        public static void GetAppTheme()
        {
            string theme = Settings.GetSetting(Settings.AppPrefrences.AppTheme);

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
            Settings.AddSetting(Settings.AppPrefrences.AppTheme, EnumsHelper.ConvertToString(Settings.Theme.LightTheme));
        }

        public static void ChangeToDarkTheme()
        {
            Application.Current.Resources = new DarkTheme();
            DependencyService.Get<IStatusBar>().ChangeStatusBarColorToBlack();
            Settings.AddSetting(Settings.AppPrefrences.AppTheme, EnumsHelper.ConvertToString(Settings.Theme.DarkTheme));
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

            Settings.AddSetting(Settings.AppPrefrences.AppTheme, EnumsHelper.ConvertToString(Settings.Theme.SystemPreferred));
        }
    }
}
