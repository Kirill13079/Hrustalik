using HealthApp.AppSettings;
using HealthApp.Common.Model;
using HealthApp.Extensions;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace HealthApp.Helpers
{
    public static class CategoriesHelper
    {
        public static List<Category> SavedUserCategories { get; private set; } = new List<Category>();

        public static List<Category> GetSavedUserCategories()
        {
            string savedUserCategories = Settings.GetSetting(prefrence: Settings.AppPrefrences.Categories);

            if (savedUserCategories != null)
            {
                SavedUserCategories = JsonConvert.DeserializeObject<List<Category>>(savedUserCategories);
            }

            return SavedUserCategories;
        }

        public static void AddUserCategory(Category category)
        {
            if (!SavedUserCategories.EqualsHelper(category))
            {
                SavedUserCategories.Add(category);
            }

            string userCategoriesJson = JsonConvert.SerializeObject(SavedUserCategories);

            Settings.AddSetting(Settings.AppPrefrences.Categories, userCategoriesJson);
        }

        public static void RemoveUserCategory(Category category)
        {
            if (SavedUserCategories.EqualsHelper(category))
            {
                _ = SavedUserCategories.Remove(category);
            }

            string userCategoriesJson = JsonConvert.SerializeObject(SavedUserCategories);

            Settings.RemoveSetting(prefrence: Settings.AppPrefrences.Categories);
            Settings.AddSetting(prefrence: Settings.AppPrefrences.Categories, setting: userCategoriesJson);
        }
    }
}
