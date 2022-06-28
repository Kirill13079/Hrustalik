using HealthApp.Models;
using HealthApp.ViewModels.Data;
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

        public static EnumType ConvertToEnum<EnumType>(this AppThemeViewModel themeModel)
        {
            return (EnumType)Enum.Parse(typeof(EnumType), themeModel.Title);
        }
    }
}
