using HealthApp.Common.Model;
using HealthApp.Common.Model.Helper;
using HealthApp.Models;
using HealthApp.Service;
using MvvmHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace HealthApp.ViewModels
{
    public class BookmarksNewsViewModel : BaseViewModel
    {
        private static BookmarksNewsViewModel _instance;
        public static BookmarksNewsViewModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new BookmarksNewsViewModel();
                }

                return _instance;
            }
        }

        private BookmarkModel _bookmarkModel;
        public BookmarkModel BookmarkModel
        {
            get => _bookmarkModel;
            set
            {
                _bookmarkModel = value;
                OnPropertyChanged();
            }
        }

        public BookmarksNewsViewModel()
        {
            BookmarkModel = new BookmarkModel();

            _ = GetDataAsync();
        }

        public async Task GetDataAsync()
        {
            if (BookmarkModel.Records.Any())
            {
                BookmarkModel.Records.Clear();
            }

            await LoadRecordsContentDataAsync().ConfigureAwait(false);
        }

        private async Task LoadRecordsContentDataAsync()
        {
            var bookmarks = await GetBookmarksAsync();

            var test = new List<Record>();

            foreach (var t in bookmarks)
            {
                test.Add(t.Record);
            }

            BookmarkModel.Records.AddRange(test);

            //foreach (var record in records)
            //{ 
            //    BookmarkModel.Records.Add(record.Record);
            //}
        }

        private async Task<List<Bookmark>> GetBookmarksAsync()
        {
            string url = ApiRoutes.BaseUrl + ApiRoutes.GetBookmarks;

            var result = await ApiCaller.Get(url);

            if (!string.IsNullOrWhiteSpace(result))
            {
                var records = JsonConvert.DeserializeObject<List<Bookmark>>(result);

                records.ForEach((record) =>
                {
                    record.Record.Image = $"{ApiRoutes.BaseUrl}/RecordImages/{record.Record.Image}";
                    record.Record.Author.Logo = $"{ApiRoutes.BaseUrl}/AuthorImages/{record.Record.Author.Logo}";
                });

                return records;
            }
            else
            {
                return null;
            }
        }
    }
}
