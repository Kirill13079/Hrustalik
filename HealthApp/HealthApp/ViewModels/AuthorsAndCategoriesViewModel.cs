using HealthApp.AppSettings;
using HealthApp.Common.Model;
using HealthApp.Common.Model.Helper;
using HealthApp.Models;
using HealthApp.Service;
using MvvmHelpers;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace HealthApp.ViewModels
{
    public class AuthorsAndCategoriesViewModel : BaseViewModel
    {
        public static AuthorsAndCategoriesViewModel Instance => new AuthorsAndCategoriesViewModel();

        private ObservableRangeCollection<TabModel> _tabAuthorsAndCategoriesItems;
        public ObservableRangeCollection<TabModel> TabAuthorsAndCategoriesItems
        {
            get => _tabAuthorsAndCategoriesItems;
            set
            {
                _tabAuthorsAndCategoriesItems = value;
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
            switch (CurrentTab.Title.ToLower())
            {
                case "категории":
                    await LoadCategoriesContentData(CurrentTab).ConfigureAwait(false);
                    break;
                case "авторы":
                    await LoadAuhorsContentData(CurrentTab).ConfigureAwait(false);
                    break;
            }
        });

        public ICommand ReloadCommand => new Command(async () =>
        {
            foreach (var tab in TabAuthorsAndCategoriesItems)
            {
                switch (tab.Title.ToLower())
                {
                    case "категории":
                        await LoadCategoriesContentData(tab).ConfigureAwait(false);
                        break;
                    case "авторы":
                        await LoadAuhorsContentData(tab).ConfigureAwait(false);
                        break;
                }
            }
        });

        public AuthorsAndCategoriesViewModel()
        {
            TabAuthorsAndCategoriesItems = new ObservableRangeCollection<TabModel>();
            CurrentTab = new TabModel();

            _ = GetData();
        }

        private async Task GetData()
        {
            if (TabAuthorsAndCategoriesItems.Any())
            {
                TabAuthorsAndCategoriesItems.Clear();
            }

            var tabModel = new ObservableRangeCollection<TabModel>()
            {
                {
                    new TabModel
                    {
                        Title = "Категории"
                    }
                },
                {
                    new TabModel
                    {
                        Title = "Авторы"
                    }
                }
            };

            await Task.Delay(250);

            TabAuthorsAndCategoriesItems = tabModel;
            CurrentTab = TabAuthorsAndCategoriesItems[0];

            foreach (var tab in TabAuthorsAndCategoriesItems)
            {
                switch (tab.Title.ToLower())
                {
                    case "категории":
                        await LoadCategoriesContentData(tab).ConfigureAwait(false);
                        break;
                    case "авторы":
                        await LoadAuhorsContentData(tab).ConfigureAwait(false);
                        break;
                }
            }
        }

        private async Task LoadAuhorsContentData(TabModel tab, bool isRefreshing = false)
        {
            tab.HasError = false;

            try
            {
                if (tab.AuthorsAndСategories.Count == 0)
                {
                    tab.IsBusy = true;

                    var authors = await GetAuthorsAsync();

                    var articles = new List<AuthorsAndCategoriesModel>();

                    foreach (var author in authors)
                    {
                        articles.Add(new AuthorsAndCategoriesModel
                        {
                            Category = null,
                            Author = author
                        });
                    }

                    tab.AuthorsAndСategories.AddRange(articles);

                    tab.IsBusy = false;
                }
                else if (isRefreshing)
                {
                    tab.IsRefreshing = true;

                    var authors = await GetAuthorsAsync();

                    var articles = new List<AuthorsAndCategoriesModel>();

                    foreach (var author in authors)
                    {
                        articles.Add(new AuthorsAndCategoriesModel
                        {
                            Category = null,
                            Author = author
                        });
                    }

                    tab.AuthorsAndСategories.ReplaceRange(articles);
                    tab.IsRefreshing = false;
                }

                if (tab.AuthorsAndСategories.Count == 0)
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

        private async Task LoadCategoriesContentData(TabModel tab, bool isRefreshing = false)
        {
            tab.HasError = false;

            try
            {
                if (tab.AuthorsAndСategories.Count == 0)
                {
                    tab.IsBusy = true;

                    var categories = await GetCategoriesAsync();

                    var articles = new List<AuthorsAndCategoriesModel>();

                    foreach (var category in categories)
                    {
                        articles.Add(new AuthorsAndCategoriesModel 
                        { 
                            Category = category, 
                            Author = null 
                        });
                    }

                    tab.AuthorsAndСategories.AddRange(articles);

                    tab.IsBusy = false;
                }
                else if (isRefreshing)
                {
                    tab.IsRefreshing = true;

                    var categories = await GetCategoriesAsync();

                    var articles = new List<AuthorsAndCategoriesModel>();

                    foreach (var category in categories)
                    {
                        articles.Add(new AuthorsAndCategoriesModel
                        {
                            Category = category,
                            Author = null
                        });
                    }

                    tab.AuthorsAndСategories.ReplaceRange(articles);
                    tab.IsRefreshing = false;
                }

                if (tab.AuthorsAndСategories.Count == 0)
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

        private async Task<List<Author>> GetAuthorsAsync()
        {
            string url = ApiRoutes.BaseUrl + ApiRoutes.GetAuthors;

            var result = await ApiCaller.Get(url);

            if (!string.IsNullOrWhiteSpace(result))
            {
                var authors = JsonConvert.DeserializeObject<List<Author>>(result);

                authors.ForEach((author) =>
                {
                    author.Logo = $"{ApiRoutes.BaseUrl}/AuthorImages/{author.Logo}";
                });

                return authors;
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

        private List<Author> GetSavedUserAuthors()
        {
            string userAuthors = Settings.GetSetting(Settings.AppPrefrences.Authors);

            if (userAuthors != null)
            {
                return JsonConvert.DeserializeObject<List<Author>>(userAuthors);
            }

            return null;
        }

        public async Task AddUserAuthors(Author author)
        {
            var savedUserAuthors = GetSavedUserAuthors();

            if (savedUserAuthors != null)
            {
                savedUserAuthors = new List<Author>();
            }

            savedUserAuthors.Add(author);

            string userAuthorsJson = JsonConvert.SerializeObject(savedUserAuthors);

            Settings.AddSetting(Settings.AppPrefrences.Authors, userAuthorsJson);
        }

        public void RemoveUserAuthors(Author author)
        {
            var savedUserAuthors = GetSavedUserAuthors();

            if (savedUserAuthors != null)
            {
                savedUserAuthors.Remove(author);

                //MainNewsViewModel.Instance.MainTabModel.Remove(removableTabItem);
            }

            string userAuthorsJson = JsonConvert.SerializeObject(savedUserAuthors);

            Settings.AddSetting(Settings.AppPrefrences.Authors, userAuthorsJson);
        }
    }
}
