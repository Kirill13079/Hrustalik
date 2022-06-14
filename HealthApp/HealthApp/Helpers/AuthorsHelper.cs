using HealthApp.AppSettings;
using HealthApp.Common.Model;
using HealthApp.Extensions;
using HealthApp.ViewModels;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace HealthApp.Helpers
{
    public static class AuthorsHelper
    {
        private static List<Author> _savedUserAuthors = new List<Author>();

        public static List<Author> SavedUserAuthors => _savedUserAuthors;

        public static List<Author> GetSavedUserAuthors()
        {
            string savedUserAuthors = Settings.GetSetting(Settings.AppPrefrences.Authors);

            if (savedUserAuthors != null)
            {
                _savedUserAuthors = JsonConvert.DeserializeObject<List<Author>>(savedUserAuthors);
            }

            return _savedUserAuthors;
        }

        public static void AddUserAuthors(Author author)
        {
            if (!_savedUserAuthors.EqualsHelper(author))
            {
                _savedUserAuthors.Add(author);
            }

            string userAuthorsJson = JsonConvert.SerializeObject(_savedUserAuthors);

            Settings.AddSetting(Settings.AppPrefrences.Authors, userAuthorsJson);

            CategoryNewsViewModel.Instance.GetData().ConfigureAwait(false);
            MainNewsViewModel.Instance.GetData().ConfigureAwait(false);
        }

        public static void RemoveUserAuthors(Author author)
        {
            if (_savedUserAuthors.EqualsHelper(author))
            {
                _savedUserAuthors.Remove(author);
            }

            string userAuthorsJson = JsonConvert.SerializeObject(_savedUserAuthors);

            Settings.RemoveSetting(Settings.AppPrefrences.Authors);
            Settings.AddSetting(Settings.AppPrefrences.Authors, userAuthorsJson);

            CategoryNewsViewModel.Instance.GetData().ConfigureAwait(false);
            MainNewsViewModel.Instance.GetData().ConfigureAwait(false);
        }
    }
}
