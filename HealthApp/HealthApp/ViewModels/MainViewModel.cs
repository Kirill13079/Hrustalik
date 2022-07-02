using HealthApp.Extensions;
using HealthApp.Helpers;
using HealthApp.Models;
using HealthApp.ViewModels.Base;
using HealthApp.ViewModels.Data;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace HealthApp.ViewModels
{
    public partial class MainViewModel : ViewBaseModel
    {
        private MainTabModel _mainTab = new MainTabModel();
        public MainTabModel MainTab
        {
            get => _mainTab;
            set
            {
                _mainTab = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            RefreshCommand = new Command(async () => await RefreshCommandHandlerAsync());
            LikeRecordCommand = new Command<RecordViewModel>(likedRecord => LikeRecordCommandHandler(likedRecord));

            _ = GetDataAsync().ConfigureAwait(false);
        }

        public async Task GetDataAsync()
        {
            CurrentState = LayoutState.Loading;

            await GetDataMainAsync();

            CurrentState = LayoutState.None;
        }

        private async Task GetDataMainAsync()
        {
            int pageSize = 2;

            if (MainTab.SubTabModel.Any())
            {
                MainTab.SubTabModel.Clear();
            }

            if (MainTab.Records.Any())
            {
                MainTab.Records.Clear();
            }

            for (int page = 0; page <= pageSize; page++)
            {
                MainTab.SubTabModel.Add(new TabModel
                {
                    Page = page
                });
            }

            MainTab.HotRecord = await ApiManager.GetHotRecordAsync();

            await LoadMainContentDataAsync().ConfigureAwait(false);

            int skipRecords = 0;

            foreach (var tab in MainTab.SubTabModel)
            {
                await LoadPopularContentDataAsync(tab, skipRecords).ConfigureAwait(false);

                skipRecords++;
            }
        }

        private async Task LoadMainContentDataAsync()
        {
            MainTab.HasError = false;

            try
            {
                MainTab.IsBusy = true;

                var records = await ApiManager.GetRecordsAsync();
                var bookmarks = await ApiManager.GetBookmarksAsync();
                var savedUserAuthors = await AuthorsHelper.GetSavedUserAuthorsAsync();
                var savedUserCategories = await CategoriesHelper.GetSavedUserCategoriesAsync();

                if (savedUserAuthors.Any())
                { 
                    _ = records.RemoveAll(match: record => !savedUserAuthors.EqualsHelper(record.Author)); 
                }

                if (savedUserCategories.Any())
                {
                    _ = records.RemoveAll(match: record => !savedUserCategories.EqualsHelper(record.Category)); 
                }

                if (bookmarks != null)
                {
                    foreach (var record in records)
                    {
                        if (bookmarks.Any(predicate: boomark => boomark.Record.Id == record.Id))
                        {
                            record.IsBookmark = true;
                        }
                    }
                }

                MainTab.Records.AddRange(records);

                MainTab.IsBusy = false;
            }
            catch
            {
                MainTab.HasError = true;
            }
            finally
            {
                MainTab.IsBusy = false;
            }
        }

        private async Task LoadPopularContentDataAsync(TabModel tab, int skipRecords, bool isRefreshing = false)
        {
            int takeRecord = 1;

            tab.HasError = false;

            try
            {
                if (tab.Records.Count == 0)
                {
                    tab.IsBusy = true;

                    var records = await ApiManager.GetPopularsRecordsAsync(skipRecords, takeRecord);
                    var savedUserAuthors = await AuthorsHelper.GetSavedUserAuthorsAsync();
                    var savedUserCategories = await CategoriesHelper .GetSavedUserCategoriesAsync();

                    if (savedUserCategories.Any())
                    {
                        _ = records.RemoveAll(match: record => !savedUserCategories.EqualsHelper(record.Category));
                    }

                    if (savedUserAuthors.Any())
                    {
                        _ = records.RemoveAll(match: record => !savedUserAuthors.EqualsHelper(record.Author));
                    }

                    tab.Records.AddRange(records);

                    tab.IsBusy = false;
                }
                else if (isRefreshing)
                {
                    tab.IsRefreshing = true;

                    var records = await ApiManager.GetPopularsRecordsAsync(skipRecords, takeRecord);
                    var savedUserAuthors = await AuthorsHelper .GetSavedUserAuthorsAsync();
                    var savedUserCategories = await CategoriesHelper .GetSavedUserCategoriesAsync();

                    if (savedUserCategories.Any())
                    {
                        _ = records.RemoveAll(match: record => !savedUserCategories.EqualsHelper(record.Category));
                    }

                    if (savedUserAuthors.Any())
                    {
                        _ = records.RemoveAll(match: record => !savedUserAuthors.EqualsHelper(record.Author));
                    }

                    tab.Records.ReplaceRange(records);

                    tab.IsRefreshing = false;
                }

                if (tab.Records.Count == 0)
                {
                    tab.IsRefreshing = false;
                    tab.IsBusy = false;
                    tab.HasError = true;
                }
            }
            catch
            {
                tab.IsRefreshing = false;
                tab.IsBusy = false;
                tab.HasError = true;
            }
        }

        private void LikeRecordCommandHandler(RecordViewModel likedRecord)
        {
            bool isLikedRecord(RecordViewModel record) => record.Equals(likedRecord);

            var mainRecord = MainTab.Records.FirstOrDefault(predicate: record => isLikedRecord(record));

            if (mainRecord != null)
            {
                mainRecord.IsBookmark = !mainRecord.IsBookmark;
            }
        }

        private async Task RefreshCommandHandlerAsync()
        {
            await LoadMainContentDataAsync();
        }
    }
}