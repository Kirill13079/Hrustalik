using HealthApp.Models;
using System;

namespace HealthApp.Helpers
{
    public static class EnumsHelper
    {
        public static string ConvertToString(this Enum eff)
        {
            return Enum.GetName(eff.GetType(), eff);
        }

        public static EnumType ConvertToEnum<EnumType>(this string enumValue)
        {
            return (EnumType)Enum.Parse(typeof(EnumType), enumValue);
        }

        public static EnumType ConvertToEnum<EnumType>(this ThemeModel themeModel)
        {
            return (EnumType)Enum.Parse(typeof(EnumType), themeModel.Title);
        }
    }
}
