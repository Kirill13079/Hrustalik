using System.Globalization;
using HealthApp.AppSettings;
using HealthApp.Extensions;
using HealthApp.Utils;
using Xamarin.CommunityToolkit.Helpers;

namespace HealthApp.Helpers
{
    public class LanguageHelper
    {
        public static void GetAppLanguage()
        {
            string language = Settings.GetSetting(prefrence: Settings.AppPrefrences.AppLanguage);

            if (language != null)
            {
                LanguageEnum.Language appLanguage = language.ConvertToEnum<LanguageEnum.Language>();

                ChangeLanguage(appLanguage);
            }
            else
            {
                ChangeLanguage(LanguageEnum.Language.Russian);
            }
        }

        public static void ChangeLanguage(LanguageEnum.Language language)
        {
            switch (language)
            {
                case LanguageEnum.Language.Russian:
                    LocalizationResourceManager.Current.CurrentCulture = new CultureInfo("ru");
                    break;
                case LanguageEnum.Language.English:
                    LocalizationResourceManager.Current.CurrentCulture = new CultureInfo("en");
                    break;
                default:
                    break;
            }

            Settings.AddSetting(prefrence: Settings.AppPrefrences.AppLanguage, setting: language.ConvertToString());
        }
    }
}
