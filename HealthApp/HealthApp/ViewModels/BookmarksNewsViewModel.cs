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
            var records = await GetBookmarkRecordsAsync();

            BookmarkModel.Records.AddRange(records);
        }

        private async Task<List<Record>> GetBookmarkRecordsAsync()
        {
            string url = ApiRoutes.BaseUrl + ApiRoutes.GetBookmarks;

            var result = await ApiCaller.Get(url);

            if (!string.IsNullOrWhiteSpace(result))
            {
                var records = JsonConvert.DeserializeObject<List<Record>>(result);

                records.ForEach((record) =>
                {
                    record.Image = $"{ApiRoutes.BaseUrl}/RecordImages/{record.Image}";
                    record.Author.Logo = $"{ApiRoutes.BaseUrl}/AuthorImages/{record.Author.Logo}";
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
