using System.Linq;
using System.Threading.Tasks;
using HealthApp.Extensions;
using HealthApp.Models;
using HealthApp.ViewModels.Base;
using HealthApp.ViewModels.Data;
using MvvmHelpers;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace HealthApp.ViewModels
{
    public class BookmarkViewModel : ViewBaseModel
    {
        private BookmarkModel _bookmarkTab = new BookmarkModel();
        public BookmarkModel BookmarkTab
        {
            get => _bookmarkTab;
            set
            {
                _bookmarkTab = value;
                OnPropertyChanged();
            }
        }

        public BookmarkViewModel()
        {
            RefreshCommand = new Command(async () => await RefreshCommandHandlerAsync());
            LikeRecordCommand = new Command<RecordViewModel>(likedRecord => LikedRecordCommandHandler(likedRecord));

            _ = GetDataAsync().ConfigureAwait(false);
        }

        public async Task GetDataAsync()
        {
            CurrentState = LayoutState.Loading;

            await GetBookamarksDataAsync();

            CurrentState = LayoutState.None;
        }

        private async Task GetBookamarksDataAsync()
        {
            if (BookmarkTab.Records.Any())
            {
                BookmarkTab.Records.Clear();
            }

            await LoadBookmarkContentDataAsync().ConfigureAwait(false);
        }

        private async Task LoadBookmarkContentDataAsync(bool isRefreshing = false)
        {
            BookmarkTab.HasError = false;
            BookmarkTab.IsEmpty = false;

            try
            {
                if (BookmarkTab.Records.Count == 0)
                {
                    if (isRefreshing)
                    {
                        BookmarkTab.IsRefreshing = true;
                    }

                    BookmarkTab.IsBusy = true;

                    var bookmarks = await ApiManager.GetBookmarksAsync();

                    var records = new ObservableRangeCollection<RecordViewModel>();

                    if (bookmarks != null)
                    {
                        foreach (var bookmark in bookmarks)
                        {
                            var record = bookmark.Record.ConvertToRecordModel();

                            record.IsBookmark = true;

                            records.Add(record);
                        }
                    }
                    else
                    {
                        BookmarkTab.HasError = true;
                    }

                    BookmarkTab.Records.AddRange(records);

                    BookmarkTab.IsBusy = false;
                }
                else if (isRefreshing)
                {
                    BookmarkTab.IsRefreshing = true;

                    var bookmarks = await ApiManager.GetBookmarksAsync();

                    var records = new ObservableRangeCollection<RecordViewModel>();

                    if (bookmarks != null)
                    {
                        foreach (var bookmark in bookmarks)
                        {
                            var record = bookmark.Record.ConvertToRecordModel();

                            record.IsBookmark = true;

                            records.Add(record);
                        }
                    }
                    else
                    {
                        BookmarkTab.HasError = true;
                    }

                    BookmarkTab.Records.ReplaceRange(records);

                    BookmarkTab.IsRefreshing = false;
                }
            }
            catch
            {
                BookmarkTab.HasError = true;
            }
            finally
            {
                BookmarkTab.IsRefreshing = false;
                BookmarkTab.IsBusy = false;
            }
        }

        private async Task RefreshCommandHandlerAsync()
        {
            await LoadBookmarkContentDataAsync(isRefreshing: true);
        }

        private void LikedRecordCommandHandler(RecordViewModel likedRecord)
        {
            bool isLikedRecord(RecordViewModel record) => record.Equals(likedRecord);

            var bookmarkRecord = BookmarkTab.Records.FirstOrDefault(predicate: record => isLikedRecord(record));

            if (bookmarkRecord != null)
            {
                _ = BookmarkTab.Records.Remove(bookmarkRecord);
            }
            else
            {
                BookmarkTab.Records.Add(likedRecord);
            }
        }
    }
}
