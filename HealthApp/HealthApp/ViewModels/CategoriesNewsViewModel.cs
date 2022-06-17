using HealthApp.Common.Model;
using HealthApp.Common.Model.Helper;
using HealthApp.Extensions;
using HealthApp.Helpers;
using HealthApp.Models;
using HealthApp.Service;
using MvvmHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace HealthApp.ViewModels
{
    public class CategoriesNewsViewModel : BaseViewModel
    {
        private List<Author> _savedUserAuthors;
        private List<Category> _savedUserCategories;

        private static CategoriesNewsViewModel _instance;
        public static CategoriesNewsViewModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CategoriesNewsViewModel();
                }

                return _instance;
            }
        }

        private ObservableRangeCollection<TabModel> _tabCategoriesRecords;
        public ObservableRangeCollection<TabModel> TabCategoriesRecords
        {
            get => _tabCategoriesRecords;
            set
            {
                _tabCategoriesRecords = value;
                OnPropertyChanged();
            }
        }

        private TabModel _currentTab;
        public TabModel CurrentTab
        {
            get => _currentTab;
            set
            {
                _currentTab = value;
                OnPropertyChanged();
            }
        }

        public ICommand RefreshCommand => new Command(async () =>
        {
            await LoadContentData(CurrentTab, isRefreshing: true);
        });

        public ICommand ReloadCommand => new Command(async () =>
        {
            foreach (var tab in TabCategoriesRecords)
            {
                await LoadContentData(tab).ConfigureAwait(false);
            }
        });

        public ICommand SelectActiveTabCommand => new Command((obj) =>
        {
            CurrentTab = (TabModel)obj;
        });

        public CategoriesNewsViewModel()
        {
            _savedUserAuthors = new List<Author>();
            _savedUserCategories = new List<Category>();
            TabCategoriesRecords = new ObservableRangeCollection<TabModel>();

            CurrentTab = new TabModel();

            _ = GetData();
        }

        public async Task GetData()
        {
            if (TabCategoriesRecords.Any())
            {
                TabCategoriesRecords.Clear();
            }

            var categories = await GetCategoriesAsync();

            _savedUserCategories = CategoriesHelper.GetSavedUserCategories();

            Predicate<Category> removedCategories = (Category category) => !_savedUserCategories.EqualsHelper(category);

            categories.RemoveAll(category => removedCategories(category));

            var tabItems = new ObservableRangeCollection<TabModel>();

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
                await LoadContentData(tab).ConfigureAwait(false);
            }
        }

        private async Task LoadContentData(TabModel tab, bool isRefreshing = false)
        {
            tab.HasError = false;

            try
            {
                if (tab.Records.Count == 0)
                {
                    tab.IsBusy = true;

                    _savedUserAuthors = AuthorsHelper.GetSavedUserAuthors();

                    var articles = await GetCategoryRecordsAsync(tab.Page);

                    Predicate<Author> removedAuthors = (Author author) => !_savedUserAuthors.EqualsHelper(author);

                    articles.RemoveAll(article => removedAuthors(article.Author));

                    tab.Records.AddRange(articles);

                    tab.IsBusy = false;
                }
                else if (isRefreshing)
                {
                    tab.IsRefreshing = true;

                    _savedUserAuthors = AuthorsHelper.GetSavedUserAuthors();

                    var articles = await GetCategoryRecordsAsync(tab.Page);

                    Predicate<Author> removedAuthors = (Author author) => !_savedUserAuthors.EqualsHelper(author);

                    articles.RemoveAll(article => removedAuthors(article.Author));

                    tab.Records.ReplaceRange(articles);

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

        private async Task<List<Record>> GetCategoryRecordsAsync(int categoryId)
        {
            string url = $"{ApiRoutes.BaseUrl}{ApiRoutes.GetCategoryRecords}?id={categoryId}";

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

        private async Task<List<Category>> GetCategoriesAsync()
        {
            string url = ApiRoutes.BaseUrl + ApiRoutes.GetCategories;

            var result = await ApiCaller.Get(url);

            if (!string.IsNullOrWhiteSpace(result))
            {
                var categories = JsonConvert.DeserializeObject<List<Category>>(result);

                return categories;
            }
            else
            {
                return null;
            }
        }
    }
}
