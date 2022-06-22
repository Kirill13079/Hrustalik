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

        public ICommand ReloadCommand => new Command(async () =>
        {
            await LoadRecordsContentDataAsync().ConfigureAwait(false);
        });

        public ICommand RefreshCommand => new Command(async () =>
        {
            await LoadRecordsContentDataAsync(isRefreshing: true);
        });

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

        private async Task LoadRecordsContentDataAsync(bool isRefreshing = false)
        {
            BookmarkModel.HasError = false;
            BookmarkModel.IsEmpty = false;

            try
            {
                if (BookmarkModel.Records.Count == 0)
                {
                    if (isRefreshing)
                    {
                        BookmarkModel.IsRefreshing = true;
                    }

                    BookmarkModel.IsBusy = true;

                    var bookmarks = await GetBookmarksAsync();

                    var records = new List<RecordModel>();

                    if (bookmarks != null)
                    {
                        foreach (var bookmark in bookmarks)
                        {
                            records.Add((RecordModel)bookmark.Record);
                        }
                    }

                    BookmarkModel.Records.AddRange(records);

                    BookmarkModel.IsBusy = false;
                }
                else if (isRefreshing)
                {
                    BookmarkModel.IsRefreshing = true;

                    var bookmarks = await GetBookmarksAsync();

                    var records = new List<RecordModel>();

                    if (bookmarks != null)
                    {
                        foreach (var bookmark in bookmarks)
                        {
                            records.Add((RecordModel)bookmark.Record);
                        }
                    }

                    BookmarkModel.Records.ReplaceRange(records);

                    BookmarkModel.IsRefreshing = false;
                }

                if (BookmarkModel.Records.Count == 0 && BookmarkModel.IsAuthorized)
                {
                    BookmarkModel.IsEmpty = true;
                }
            }
            catch 
            {
                BookmarkModel.HasError = true;
            }
            finally
            {
                BookmarkModel.IsRefreshing = false;
                BookmarkModel.IsBusy = false;
            }
        }

        private async Task<List<Bookmark>> GetBookmarksAsync()
        {
            string url = ApiRoutes.BaseUrl + ApiRoutes.GetBookmarks;

            var result = await ApiCaller.Get(url);

            if (!string.IsNullOrWhiteSpace(result))
            {
                var bookmarks = JsonConvert.DeserializeObject<List<Bookmark>>(result);

                bookmarks.ForEach((bookmark) =>
                {
                    bookmark.Record.Image = $"{ApiRoutes.BaseUrl}/RecordImages/{bookmark.Record.Image}";
                    bookmark.Record.Author.Logo = $"{ApiRoutes.BaseUrl}/AuthorImages/{bookmark.Record.Author.Logo}";
                });

                BookmarkModel.IsAuthorized = true;

                return bookmarks;
            }
            else
            {
                BookmarkModel.IsAuthorized = false;

                return null;
            }
        }
    }
}
