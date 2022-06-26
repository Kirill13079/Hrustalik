using HealthApp.AppSettings;
using HealthApp.Common.Model;
using HealthApp.Extensions;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace HealthApp.Helpers
{
    public static class AuthorsHelper
    {
        public static List<Author> SavedUserAuthors { get; private set; } = new List<Author>();

        public static List<Author> GetSavedUserAuthors()
        {
            string savedUserAuthors = Settings.GetSetting(prefrence: Settings.AppPrefrences.Authors);

            if (savedUserAuthors != null)
            {
                SavedUserAuthors = JsonConvert.DeserializeObject<List<Author>>(savedUserAuthors);
            }

            return SavedUserAuthors;
        }

        public static void AddUserAuthors(Author author)
        {
            if (!SavedUserAuthors.EqualsHelper(author))
            {
                SavedUserAuthors.Add(author);
            }

            string userAuthorsJson = JsonConvert.SerializeObject(SavedUserAuthors);

            Settings.AddSetting(prefrence: Settings.AppPrefrences.Authors, setting: userAuthorsJson);
        }

        public static void RemoveUserAuthors(Author author)
        {
            if (SavedUserAuthors.EqualsHelper(author))
            {
                _ = SavedUserAuthors.Remove(author);
            }

            string userAuthorsJson = JsonConvert.SerializeObject(SavedUserAuthors);

            Settings.RemoveSetting(prefrence: Settings.AppPrefrences.Authors);
            Settings.AddSetting(prefrence: Settings.AppPrefrences.Authors, setting: userAuthorsJson);
        }
    }
}
