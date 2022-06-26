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
            Categories,
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
            Preferences.Set(key: EnumsHelper.ConvertToString(prefrence), value: setting);
        }

        public static string GetSetting(AppPrefrences prefrence)
        {
            bool hasKey = Preferences.ContainsKey(key: EnumsHelper.ConvertToString(eff: prefrence));

            return hasKey ? Preferences.Get(key: EnumsHelper.ConvertToString(prefrence), defaultValue: null) : null;
        }

        public static void RemoveSetting(AppPrefrences prefrence)
        {
            Preferences.Remove(EnumsHelper.ConvertToString(eff: prefrence));
        }

        public static void ClearSettings()
        {
            Preferences.Clear();
        }

        public static async Task SetSecureSetting(AppPrefrences prefrence, string setting)
        {
            try
            {
                await SecureStorage.SetAsync(key: EnumsHelper.ConvertToString(prefrence), value: setting);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public static async Task<string> GetSecureSetting(AppPrefrences prefrence)
        {
            return await SecureStorage.GetAsync(key: EnumsHelper.ConvertToString(eff: prefrence));
        }

        public static void RemoveSecureSetting(AppPrefrences prefrence)
        {
            _ = SecureStorage.Remove(key: EnumsHelper.ConvertToString(eff: prefrence));
        }

        public static void ClearSecureSorage()
        {
            SecureStorage.RemoveAll();
        }
    }
}
