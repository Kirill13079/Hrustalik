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
    public static class AuthorsHelper
    {
        private static readonly IApiManager _apiManager = new ApiManager();

        public static List<Author> SavedUserAuthors { get; private set; } = new List<Author>();

        public async static Task<List<Author>> GetSavedUserAuthorsAsync()
        {
            string savedUserAuthors = Settings.GetSetting(prefrence: Settings.AppPrefrences.Authors);

            if (savedUserAuthors != null)
            {
                SavedUserAuthors = JsonConvert.DeserializeObject<List<Author>>(savedUserAuthors);
            }
            else
            {
                var authors = await _apiManager.GetAuthorsAsync();

                if (authors != null)
                {
                    string userAuthorsDefaultJson = JsonConvert.SerializeObject(authors);

                    Settings.AddSetting(prefrence: Settings.AppPrefrences.Authors, setting: userAuthorsDefaultJson);

                    SavedUserAuthors = authors;
                }
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
