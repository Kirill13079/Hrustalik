using HealthApp.Extensions;
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

        public static void AddSetting(AppPrefrences prefrence, string setting)
        {
            Preferences.Set(key: EnumExtension.ConvertToString(prefrence), value: setting);
        }

        public static string GetSetting(AppPrefrences prefrence)
        {
            bool hasKey = Preferences.ContainsKey(key: EnumExtension.ConvertToString(prefrence));

            return hasKey ? Preferences.Get(key: EnumExtension.ConvertToString(prefrence), defaultValue: null) : null;
        }

        public static void RemoveSetting(AppPrefrences prefrence)
        {
            Preferences.Remove(EnumExtension.ConvertToString(prefrence));
        }

        public static void ClearSettings()
        {
            Preferences.Clear();
        }

        public static async Task SetSecureSetting(AppPrefrences prefrence, string setting)
        {
            try
            {
                await SecureStorage.SetAsync(key: EnumExtension.ConvertToString(prefrence), value: setting);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public static async Task<string> GetSecureSetting(AppPrefrences prefrence)
        {
            return await SecureStorage.GetAsync(key: EnumExtension.ConvertToString(prefrence));
        }

        public static void RemoveSecureSetting(AppPrefrences prefrence)
        {
            _ = SecureStorage.Remove(key: EnumExtension.ConvertToString(prefrence));
        }

        public static void ClearSecureSorage()
        {
            SecureStorage.RemoveAll();
        }
    }
}
