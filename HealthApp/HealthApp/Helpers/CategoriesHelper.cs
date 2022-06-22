using HealthApp.AppSettings;
using HealthApp.Common.Model;
using HealthApp.Extensions;
using HealthApp.ViewModels;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace HealthApp.Helpers
{
    public static class CategoriesHelper
    {
        private static List<Category> _savedUserCategories = new List<Category>();

        public static List<Category> SavedUserCategories => _savedUserCategories;

        public static List<Category> GetSavedUserCategories()
        {
            string savedUserCategories = Settings.GetSetting(Settings.AppPrefrences.Categories);

            if (savedUserCategories != null)
            {
                _savedUserCategories = JsonConvert.DeserializeObject<List<Category>>(savedUserCategories);
            }

            return _savedUserCategories;
        }

        public static void AddUserCategory(Category category)
        {
            if (!_savedUserCategories.EqualsHelper(category))
            {
                _savedUserCategories.Add(category);
            }

            string userCategoriesJson = JsonConvert.SerializeObject(_savedUserCategories);

            Settings.AddSetting(Settings.AppPrefrences.Categories, userCategoriesJson);

            CategoriesNewsViewModel.Instance.GetDataAsync().ConfigureAwait(false);
            MainNewsViewModel.Instance.GetData().ConfigureAwait(false);
        }

        public static void RemoveUserCategory(Category category)
        {
            if (_savedUserCategories.EqualsHelper(category))
            {
                _savedUserCategories.Remove(category);
            }

            string userCategoriesJson = JsonConvert.SerializeObject(_savedUserCategories);

            Settings.RemoveSetting(Settings.AppPrefrences.Categories);
            Settings.AddSetting(Settings.AppPrefrences.Categories, userCategoriesJson);

            CategoriesNewsViewModel.Instance.GetDataAsync().ConfigureAwait(false);
            MainNewsViewModel.Instance.GetData().ConfigureAwait(false);
        }
    }
}
