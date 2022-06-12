using HealthApp.Helpers;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace HealthApp.AppSettings
{
    public static class Settings
    {
        public enum AppPrefrences
        {
            AppTheme,
            User,
            Authors,
            token
        }

        public enum Theme
        {
            LightTheme,
            DarkTheme,
            SystemPreferred
        }

        public static void AddSetting(AppPrefrences prefrence, string setting)
        {
            Preferences.Set(EnumsHelper.ConvertToString(prefrence), setting);
        }

        public static string GetSetting(AppPrefrences prefrence)
        {
            bool hasKey = Preferences.ContainsKey(EnumsHelper.ConvertToString(prefrence));

            if (hasKey)
            {
                return Preferences.Get(EnumsHelper.ConvertToString(prefrence), null);
            }

            return null;
        }

        public static void RemoveSetting(AppPrefrences prefrence)
        {
            Preferences.Remove(EnumsHelper.ConvertToString(prefrence));
        }

        public static void ClearSettings()
        {
            Preferences.Clear();
        }

        public static async Task SetSecureSetting(AppPrefrences prefrence, string setting)
        {
            try
            {
                await SecureStorage.SetAsync(EnumsHelper.ConvertToString(prefrence), setting);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public static async Task<string> GetSecureSetting(AppPrefrences prefrence)
        {
            return await SecureStorage.GetAsync(EnumsHelper.ConvertToString(prefrence));
        }

        public static void RemoveSecureSetting(AppPrefrences prefrence)
        {
            SecureStorage.Remove(EnumsHelper.ConvertToString(prefrence));
        }

        public static void ClearSecureSorage()
        {
            SecureStorage.RemoveAll();
        }
    }
}
