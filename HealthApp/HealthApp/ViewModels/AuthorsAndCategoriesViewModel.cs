using HealthApp.AppSettings;
using HealthApp.Common.Model;
using HealthApp.Common.Model.Helper;
using HealthApp.Extensions;
using HealthApp.Helpers;
using HealthApp.Models;
using HealthApp.Service;
using HealthApp.ViewModels.Data;
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
        private ObservableRangeCollection<AuthorAndCategoryModel> _tabAuthorsAndCategoriesItems;
        public ObservableRangeCollection<AuthorAndCategoryModel> TabAuthorsAndCategoriesItems
        {
            get => _tabAuthorsAndCategoriesItems;
            set
            {
                _tabAuthorsAndCategoriesItems = value;
                OnPropertyChanged();
            }
        }

        private AuthorAndCategoryModel _currentTab;
        public AuthorAndCategoryModel CurrentTab
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
            TabAuthorsAndCategoriesItems = new ObservableRangeCollection<AuthorAndCategoryModel>();
            CurrentTab = new AuthorAndCategoryModel();

            _ = GetDataAsync().ConfigureAwait(false);
        }

        private async Task GetDataAsync()
        {
            if (TabAuthorsAndCategoriesItems.Any())
            {
                TabAuthorsAndCategoriesItems.Clear();
            }

            var tabModel = new ObservableRangeCollection<AuthorAndCategoryModel>()
            {
                {
                    new AuthorAndCategoryModel
                    {
                        Title = "Категории"
                    }
                },
                {
                    new AuthorAndCategoryModel
                    {
                        Title = "Авторы"
                    }
                }
            };

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

        private async Task LoadAuhorsContentData(AuthorAndCategoryModel tab, bool isRefreshing = false)
        {
            tab.HasError = false;

            try
            {
                if (tab.AuthorsAndСategories.Count == 0)
                {
                    tab.IsBusy = true;

                    var authors = await GetAuthorsAsync();

                    var savedUserAuthors = AuthorsHelper.GetSavedUserAuthors();

                    var articles = new List<AuthorAndCategoryViewModel>();

                    foreach (var author in authors)
                    {
                        var article = new AuthorAndCategoryViewModel
                        {
                            Category = null, 
                            Author = author 
                        };

                        if (savedUserAuthors.EqualsHelper(article.Author))
                        {
                            article.IsActive = true;
                        }

                        articles.Add(article);
                    }

                    tab.AuthorsAndСategories.AddRange(articles);

                    tab.IsBusy = false;
                }
                else if (isRefreshing)
                {
                    tab.IsRefreshing = true;

                    var authors = await GetAuthorsAsync();

                    var savedUserAuthors = AuthorsHelper.GetSavedUserAuthors();

                    var articles = new List<AuthorAndCategoryViewModel>();

                    foreach (var author in authors)
                    {
                        var article = new AuthorAndCategoryViewModel
                        {
                            Category = null,
                            Author = author
                        };

                        if (savedUserAuthors.EqualsHelper(article.Author))
                        {
                            article.IsActive = true;
                        }

                        articles.Add(article);
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

        private async Task LoadCategoriesContentData(AuthorAndCategoryModel tab, bool isRefreshing = false)
        {
            tab.HasError = false;

            try
            {
                if (tab.AuthorsAndСategories.Count == 0)
                {
                    tab.IsBusy = true;

                    var categories = await GetCategoriesAsync();

                    var savedUserCategories = CategoriesHelper.GetSavedUserCategories();

                    var articles = new List<AuthorAndCategoryViewModel>();

                    foreach (var category in categories)
                    {
                        var article = new AuthorAndCategoryViewModel
                        {
                            Category = category,
                            Author = null
                        };

                        if (savedUserCategories.EqualsHelper(article.Category))
                        {
                            article.IsActive = true;
                        }

                        articles.Add(article);
                    }

                    tab.AuthorsAndСategories.AddRange(articles);

                    tab.IsBusy = false;
                }
                else if (isRefreshing)
                {
                    tab.IsRefreshing = true;

                    var categories = await GetCategoriesAsync();

                    var savedUserCategories = CategoriesHelper.GetSavedUserCategories();

                    var articles = new List<AuthorAndCategoryViewModel>();

                    foreach (var category in categories)
                    {
                        var article = new AuthorAndCategoryViewModel
                        {
                            Category = category,
                            Author = null
                        };

                        if (savedUserCategories.EqualsHelper(article.Category))
                        {
                            article.IsActive = true;
                        }

                        articles.Add(article);
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

                categories.ForEach((category) =>
                {
                    //category.Image = $"{ApiRoutes.BaseUrl}/AuthorImages/{author.Logo}";
                });

                return categories;
            }
            else
            {
                return null;
            }
        }
    }
}
