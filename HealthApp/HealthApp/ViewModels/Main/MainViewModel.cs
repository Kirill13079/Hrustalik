using HealthApp.Common.Model;
using HealthApp.Extensions;
using HealthApp.Helpers;
using HealthApp.Interfaces;
using HealthApp.Models;
using HealthApp.Service;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;

namespace HealthApp.ViewModels.Main
{
    public partial class MainViewModel : BaseViewModel
    {
        private List<Author> _savedUserAuthors = new List<Author>();
        private List<Category> _savedUserCategories = new List<Category>();

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

        public MainViewModel()
        {
            _ = GetDataAsync().ConfigureAwait(false);
        }

        public async Task GetDataAsync()
        {
            CurrentState = LayoutState.Loading;

            if (_savedUserAuthors.Any())
            {
                _savedUserAuthors.Clear();
            }

            if (_savedUserCategories.Any())
            {
                _savedUserCategories.Clear();
            }

            await Task.WhenAll(GetDataMainNewsLoad(), GetDataCategoriesNewsLoad());

            CurrentState = LayoutState.None;
        }

        public void SetLikeRecord(Record activeRecord, bool isLiked)
        {
            var mainRecord = MainTabModel.Records.FirstOrDefault(record => record.Equals(activeRecord));

            if (mainRecord != null)
            {
                mainRecord.IsBookmark = isLiked;
            }

            foreach (var tab in TabCategoriesRecords)
            {
                if (tab.Records.Count > 0)
                {
                    var categoryRecord = tab.Records.FirstOrDefault(recod => recod.Equals(activeRecord));

                    if (categoryRecord != null)
                    {
                        categoryRecord.IsBookmark = isLiked;
                    }
                }
            }
        }

        private async Task GetDataMainNewsLoad()
        {
            if (MainTabModel.SubTabModel.Any())
            {
                MainTabModel.SubTabModel.Clear();
            }

            if (MainTabModel.Records.Any())
            {
                MainTabModel.Records.Clear();
            }

            for (int page = 0; page <= _pageSize; page++)
            {
                MainTabModel.SubTabModel.Add(new TabModel
                {
                    Page = page
                });
            }

            MainTabModel.HotRecord = await ApiManager.GetHotRecordAsync().ConfigureAwait(false);

            await LoadMainNewsRecordsContentData().ConfigureAwait(false);

            _skipRecords = 0;

            foreach (var tab in MainTabModel.SubTabModel)
            {
                _skipRecords++;

                await LoadPopularMainNewsRecordsContentData(tab).ConfigureAwait(false);
            }
        }

        private async Task GetDataCategoriesNewsLoad()
        {
            if (TabCategoriesRecords.Any())
            {
                TabCategoriesRecords.Clear();
            }

            var categories = await ApiManager.GetCategoriesAsync();
            var tabItems = new ObservableRangeCollection<TabModel>();

            _savedUserCategories = CategoriesHelper.GetSavedUserCategories();

            _ = categories.RemoveAll(match: category => !_savedUserCategories.EqualsHelper(category));

            foreach (var category in categories)
            {
                tabItems.Add(new TabModel
                {
                    Title = category.Name,
                    Page = category.Id
                });
            }

            TabCategoriesRecords = tabItems;
            CurrentTab = TabCategoriesRecords[0];

            foreach (var tab in TabCategoriesRecords)
            {
                await LoadCategoriesNewsViewContentData(tab).ConfigureAwait(false);
            }
        }
    }
}
