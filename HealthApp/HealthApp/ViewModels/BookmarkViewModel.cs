using HealthApp.Common.Model;
using HealthApp.Common.Model.Helper;
using HealthApp.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace HealthApp.ViewModels
{
    public class BookmarkViewModel : BaseViewModel
    {
        public List<Bookmark> Bookmarks { get; set; } = new List<Bookmark>();

        public ICommand RefreshCommand => new Command(LoadBookmarks);

        private async void LoadBookmarks()
        {
            var bookmarks = await GetBookmarksAsync();

            Bookmarks = bookmarks;
        }

        private async Task<List<Bookmark>> GetBookmarksAsync()
        {
            string url = BaseUrl + ApiRoutes.GetBookmarks;

            var result = await ApiCaller.Get(url);

            if (!string.IsNullOrWhiteSpace(result))
            {
                var bookmarks = JsonConvert.DeserializeObject<List<Bookmark>>(result);

                bookmarks.ForEach((bookmark) =>
                {
                    bookmark.Record.Image = $"{BaseUrl}/RecordImages/{bookmark.Record.Image}";
                    bookmark.Record.Author.Logo = $"{BaseUrl}/AuthorImages/{bookmark.Record.Author.Logo}";
                });

                return bookmarks;
            }
            else
            {
                return null;
            }
        }
    }
}
