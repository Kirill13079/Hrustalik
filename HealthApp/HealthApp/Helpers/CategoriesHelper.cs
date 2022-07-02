using HealthApp.AppSettings;
using HealthApp.Common.Model;
using HealthApp.Extensions;
using HealthApp.Interfaces;
using HealthApp.Service;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthApp.Helpers
{
    public static class CategoriesHelper
    {
        private static readonly IApiManager _apiManager = new ApiManager();

        public static List<Category> SavedUserCategories { get; private set; } = new List<Category>();

        public async static Task<List<Category>> GetSavedUserCategoriesAsync()
        {
            string savedUserCategories = Settings.GetSetting(prefrence: Settings.AppPrefrences.Categories);

            if (savedUserCategories != null)
            {
                SavedUserCategories = JsonConvert.DeserializeObject<List<Category>>(savedUserCategories);
            }
            else 
            {
                var categories = await _apiManager.GetCategoriesAsync();

                if (categories != null)
                {
                    string userCategoriesDefaultJson = JsonConvert.SerializeObject(categories);

                    Settings.AddSetting(prefrence: Settings.AppPrefrences.Categories, setting: userCategoriesDefaultJson);

                    SavedUserCategories = categories;
                }
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
