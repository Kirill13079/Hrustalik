using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Resources;
using HealthApp.ViewModels.Data;

namespace HealthApp.Extensions
{
    public static class EnumExtension
    {
        public static string DisplayName(this Enum enumValue)
        {
            Type enumType = enumValue.GetType();

            MemberInfo memberInfo = enumType.GetMember(enumValue.ToString()).First();

            if (memberInfo == null || !memberInfo.CustomAttributes.Any())
            {
                return enumValue.ToString();
            }

            DisplayAttribute displayAttribute = memberInfo.GetCustomAttribute<DisplayAttribute>();

            if (displayAttribute == null)
            {
                return enumValue.ToString();
            }

            if (displayAttribute.ResourceType != null && displayAttribute.Name != null)
            {
                ResourceManager manager = new ResourceManager(displayAttribute.ResourceType);

                return manager.GetString(displayAttribute.Name);
            }

            return displayAttribute.Name ?? enumValue.ToString();
        }

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
