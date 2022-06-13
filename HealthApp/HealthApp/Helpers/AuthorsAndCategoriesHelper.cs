using HealthApp.AppSettings;
using HealthApp.Common.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthApp.Helpers
{
    public static class AuthorsAndCategoriesHelper
    {
        public static List<Author> GetSavedUserAuthors()
        {
            string savedUserAuthors = Settings.GetSetting(Settings.AppPrefrences.Authors);

            if (savedUserAuthors != null)
            {
                var authors = JsonConvert.DeserializeObject<List<Author>>(savedUserAuthors);

                return authors;
            }

            return new List<Author>();
        }

        public static void AddUserAuthors(Author author)
        {
            var savedUserAuthors = GetSavedUserAuthors();

            if (savedUserAuthors.Where(x => x.Id == author.Id).Count() == 0)
            { 
                savedUserAuthors.Add(author); 
            }

            string userAuthorsJson = JsonConvert.SerializeObject(savedUserAuthors);

            Settings.AddSetting(Settings.AppPrefrences.Authors, userAuthorsJson);
        }

        public static void RemoveUserAuthors(Author author)
        {
            var savedUserAuthors = GetSavedUserAuthors();

            var savedAuthor = savedUserAuthors
                .Where(x => x.Id == author.Id)
                .FirstOrDefault();

            if (savedAuthor != null)
            {
                savedUserAuthors.Remove(savedAuthor);
            }

            string userAuthorsJson = JsonConvert.SerializeObject(savedUserAuthors);

            Settings.RemoveSetting(Settings.AppPrefrences.Authors);
            Settings.AddSetting(Settings.AppPrefrences.Authors, userAuthorsJson);
        }
    }
}
