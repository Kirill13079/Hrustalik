using HealthApp.Extensions;
using HealthApp.Helpers;
using HealthApp.Interfaces;
using HealthApp.Models;
using HealthApp.Service;
using MvvmHelpers;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace HealthApp.ViewModels
{
    public partial class MainViewModel : BaseViewModel
    {
        protected IApiManager ApiManager = new ApiManager();

        private static MainViewModel _instance;
        public static MainViewModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MainViewModel();
                }

                return _instance;
            }
        }

        private MainTabModel _mainTabModel = new MainTabModel();
        public MainTabModel MainTabModel
        {
            get => _mainTabModel;
            set
            {
                if (_mainTabModel != null)
                {
                    _mainTabModel = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableRangeCollection<TabModel> _tabCategories = new ObservableRangeCollection<TabModel>();
        public ObservableRangeCollection<TabModel> TabCategories
        {
            get => _tabCategories;
            set
            {
                if (_tabCategories != null)
                {
                    _tabCategories = value;
                    OnPropertyChanged();
                }
            }
        }

        private TabModel _currentCategoryTab = new TabModel();
        public TabModel CurrentCategoryTab
        {
            get => _currentCategoryTab;
            set
            {
                if (_currentCategoryTab != null)
                {
                    _currentCategoryTab = value;
                    OnPropertyChanged();
                }
            }
        }

        private LayoutState _currentState = LayoutState.Loading;
        public LayoutState CurrentState
        {
            get => _currentState;
            set
            {
                if (_currentState != value)
                {
                    _currentState = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand LikeRecordCommand => new Command<RecordModel>(likedRecord =>
        {
            var mainRecord = MainTabModel.Records.FirstOrDefault(record => record.Equals(likedRecord));

            if (mainRecord != null)
            {
                mainRecord.IsBookmark = !mainRecord.IsBookmark;
            }

            foreach (var tab in TabCategories)
            {
                if (tab.Records.Count > 0)
                {
                    var categoryRecord = tab.Records.FirstOrDefault(predicate: recod => recod.Equals(likedRecord));

                    if (categoryRecord != null)
                    {
                        categoryRecord.IsBookmark = !categoryRecord.IsBookmark;
                    }
                }
            }
        });

        public ICommand RefreshCategoryCommand => new Command(async () =>
        {
            await LoadCategoriesNewsViewContentData(CurrentCategoryTab, isRefreshing: true);
        });

        public ICommand ReloadCategoryCommand => new Command(async () =>
        {
            foreach (var tab in TabCategories)
            {
                await LoadCategoriesNewsViewContentData(tab).ConfigureAwait(false);
            }
        });

        public MainViewModel()
        {
            _ = GetDataAsync().ConfigureAwait(false);
        }

        public async Task GetDataAsync()
        {
            CurrentState = LayoutState.Loading;

            await Task.WhenAll(GetDataMainNewsLoadaAsync(), GetDataCategoriesNewsLoadAsync());

            CurrentState = LayoutState.None;
        }

        private async Task GetDataMainNewsLoadaAsync()
        {
            int pageSize = 2;

            if (MainTabModel.SubTabModel.Any())
            {
                MainTabModel.SubTabModel.Clear();
            }

            if (MainTabModel.Records.Any())
            {
                MainTabModel.Records.Clear();
            }

            for (int page = 0; page <= pageSize; page++)
            {
                MainTabModel.SubTabModel.Add(new TabModel
                {
                    Page = page
                });
            }

            MainTabModel.HotRecord = await ApiManager.GetHotRecordAsync().ConfigureAwait(false);

            await LoadMainNewsContentDataAsync().ConfigureAwait(false);

            int skipRecords = 0;

            foreach (var tab in MainTabModel.SubTabModel)
            {
                skipRecords++;

                await LoadPopularsRecordsContentDataAsync(tab, skipRecords).ConfigureAwait(false);
            }
        }

        private async Task GetDataCategoriesNewsLoadAsync()
        {
            if (TabCategories.Any())
            {
                TabCategories.Clear();
            }

            var categories = await ApiManager.GetCategoriesAsync();

            var tabItems = new ObservableRangeCollection<TabModel>();

            var savedUserCategories = CategoriesHelper.GetSavedUserCategories();

            _ = categories.RemoveAll(match: category => !savedUserCategories.EqualsHelper(category));

            foreach (var category in categories)
            {
                tabItems.Add(new TabModel
                {
                    Title = category.Name,
                    Page = category.Id
                });
            }

            TabCategories = tabItems;
            CurrentCategoryTab = TabCategories[0];

            foreach (var tab in TabCategories)
            {
                await LoadCategoriesNewsViewContentData(tab).ConfigureAwait(false);
            }
        }

        private async Task LoadMainNewsContentDataAsync()
        {
            MainTabModel.HasError = false;

            try
            {
                MainTabModel.IsBusy = true;

                var records = await ApiManager.GetRecordsAsync();
                var bookmarks = await ApiManager.GetBookmarksAsync();

                var savedUserAuthors = AuthorsHelper.GetSavedUserAuthors();
                var savedUserCategories = CategoriesHelper.GetSavedUserCategories();

                _ = records.RemoveAll(match: record => !savedUserAuthors.EqualsHelper(record.Author));
                _ = records.RemoveAll(match: record => !savedUserCategories.EqualsHelper(record.Category));

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

                MainTabModel.Records.AddRange(records);

                MainTabModel.IsBusy = false;
            }
            catch
            {
                MainTabModel.HasError = true;
            }
            finally
            {
                MainTabModel.IsBusy = false;
            }
        }

        private async Task LoadPopularsRecordsContentDataAsync(TabModel tab, int skipRecords, bool isRefreshing = false)
        {
            int takeRecord = 1;

            tab.HasError = false;

            try
            {
                if (tab.Records.Count == 0)
                {
                    tab.IsBusy = true;

                    var records = await ApiManager.GetPopularsRecordsAsync(skipRecords, takeRecord);

                    var savedUserAuthors = AuthorsHelper.GetSavedUserAuthors();
                    var savedUserCategories = CategoriesHelper.GetSavedUserCategories();

                    _ = records.RemoveAll(match: record => !savedUserCategories.EqualsHelper(record.Category));
                    _ = records.RemoveAll(match: record => !savedUserAuthors.EqualsHelper(record.Author));

                    tab.Records.AddRange(records);

                    tab.IsBusy = false;
                }
                else if (isRefreshing)
                {
                    tab.IsRefreshing = true;

                    var records = await ApiManager.GetPopularsRecordsAsync(skipRecords, takeRecord);

                    var savedUserAuthors = AuthorsHelper.GetSavedUserAuthors();
                    var savedUserCategories = CategoriesHelper.GetSavedUserCategories();

                    _ = records.RemoveAll(match: record => !savedUserCategories.EqualsHelper(record.Category));
                    _ = records.RemoveAll(match: record => !savedUserAuthors.EqualsHelper(record.Author));

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

        private async Task LoadCategoriesNewsViewContentData(TabModel tab, bool isRefreshing = false)
        {
            tab.HasError = false;

            try
            {
                if (tab.Records.Count == 0)
                {
                    if (isRefreshing)
                    {
                        tab.IsRefreshing = true;
                    }

                    tab.IsBusy = true;

                    var savedUserAuthors = AuthorsHelper.GetSavedUserAuthors();

                    var bookmarks = await ApiManager.GetBookmarksAsync();
                    var records = await ApiManager.GetCategoryRecordsAsync(categoryId: tab.Page);

                    _ = records.RemoveAll(match: record => !savedUserAuthors.EqualsHelper(record.Author));

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

                    tab.Records.AddRange(records);

                    tab.IsBusy = false;
                }
                else if (isRefreshing)
                {
                    tab.IsRefreshing = true;

                    var savedUserAuthors = AuthorsHelper.GetSavedUserAuthors();

                    var bookmarks = await ApiManager.GetBookmarksAsync();
                    var records = await ApiManager.GetCategoryRecordsAsync(categoryId: tab.Page);

                    _ = records.RemoveAll(match: record => !savedUserAuthors.EqualsHelper(record.Author));

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

                    tab.Records.ReplaceRange(records);

                    tab.IsRefreshing = false;
                }

                if (tab.Records.Count == 0)
                {
                    tab.HasError = true;
                }
            }
            catch
            {
                tab.HasError = true;
            }
            finally
            {
                tab.IsRefreshing = false;
                tab.IsBusy = false;
            }
        }
    }
}
