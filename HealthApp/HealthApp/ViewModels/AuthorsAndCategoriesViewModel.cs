using HealthApp.Common.Model;
using HealthApp.Extensions;
using HealthApp.Helpers;
using HealthApp.Models;
using HealthApp.ViewModels.Base;
using HealthApp.ViewModels.Data;
using MvvmHelpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HealthApp.ViewModels
{
    public class AuthorsAndCategoriesViewModel : ViewBaseModel
    {
        private ObservableRangeCollection<AuthorAndCategoryModel> _tabAuthorsAndCategoriesItems = new ObservableRangeCollection<AuthorAndCategoryModel>();
        public ObservableRangeCollection<AuthorAndCategoryModel> TabAuthorsAndCategoriesItems
        {
            get => _tabAuthorsAndCategoriesItems;
            set
            {
                _tabAuthorsAndCategoriesItems = value;
                OnPropertyChanged();
            }
        }

        private AuthorAndCategoryModel _currentTab = new AuthorAndCategoryModel();
        public AuthorAndCategoryModel CurrentTab
        {
            get => _currentTab;
            set
            {
                _currentTab = value;
                OnPropertyChanged();
            }
        }

        public AuthorsAndCategoriesViewModel()
        {
            ReloadCommand = new Command(async () => await ReloadCommandHandlerAsync());
            RefreshCommand = new Command(async () => await RefreshCommandHandlerAsync());

            _ = GetDataAsync().ConfigureAwait(false);
        }

        private async Task GetDataAsync()
        {
            if (TabAuthorsAndCategoriesItems.Any())
            {
                TabAuthorsAndCategoriesItems.Clear();
            }

            SetTabAuthorsAndCategoriesItems();

            CurrentTab = TabAuthorsAndCategoriesItems[0];

            foreach (AuthorAndCategoryModel tab in TabAuthorsAndCategoriesItems)
            {
                switch (tab.Title.ToLower())
                {
                    case "категории":
                        await LoadCategoryContentDataAsync(tab).ConfigureAwait(false);
                        break;
                    case "авторы":
                        await LoadAuhorContentDataAsync(tab).ConfigureAwait(false);
                        break;
                    default:
                        break;
                }
            }
        }

        private async Task LoadAuhorContentDataAsync(AuthorAndCategoryModel tab, bool isRefreshing = false)
        {
            tab.HasError = false;

            try
            {
                if (tab.AuthorsAndСategories.Count == 0)
                {
                    tab.IsBusy = true;

                    List<Author> authors = await ApiManagerService.GetAuthorsAsync();
                    List<Author> savedUserAuthors = await AuthorsHelper.GetSavedUserAuthorsAsync();

                    ObservableRangeCollection<AuthorAndCategoryViewModel> articles = new ObservableRangeCollection<AuthorAndCategoryViewModel>();

                    foreach (Author author in authors)
                    {
                        AuthorAndCategoryViewModel article = new AuthorAndCategoryViewModel
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

                    var authors = await ApiManagerService.GetAuthorsAsync();
                    var savedUserAuthors = await AuthorsHelper.GetSavedUserAuthorsAsync();

                    var articles = new ObservableRangeCollection<AuthorAndCategoryViewModel>();

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

        private async Task LoadCategoryContentDataAsync(AuthorAndCategoryModel tab, bool isRefreshing = false)
        {
            tab.HasError = false;

            try
            {
                if (tab.AuthorsAndСategories.Count == 0)
                {
                    tab.IsBusy = true;

                    List<Category> categories = await ApiManagerService.GetCategoriesAsync();
                    List<Category> savedUserCategories = await CategoriesHelper.GetSavedUserCategoriesAsync();

                    ObservableRangeCollection<AuthorAndCategoryViewModel> articles = new ObservableRangeCollection<AuthorAndCategoryViewModel>();

                    foreach (Category category in categories)
                    {
                        AuthorAndCategoryViewModel article = new AuthorAndCategoryViewModel
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

                    List<Category> categories = await ApiManagerService.GetCategoriesAsync();
                    List<Category> savedUserCategories = await CategoriesHelper.GetSavedUserCategoriesAsync();

                    ObservableRangeCollection<AuthorAndCategoryViewModel> articles = new ObservableRangeCollection<AuthorAndCategoryViewModel>();

                    foreach (Category category in categories)
                    {
                        AuthorAndCategoryViewModel article = new AuthorAndCategoryViewModel
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

        private void SetTabAuthorsAndCategoriesItems()
        {
            TabAuthorsAndCategoriesItems = new ObservableRangeCollection<AuthorAndCategoryModel>()
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
        }

        private async Task ReloadCommandHandlerAsync()
        {
            foreach (AuthorAndCategoryModel tab in TabAuthorsAndCategoriesItems)
            {
                switch (tab.Title.ToLower())
                {
                    case "категории":
                        await LoadCategoryContentDataAsync(tab).ConfigureAwait(false);
                        break;
                    case "авторы":
                        await LoadAuhorContentDataAsync(tab).ConfigureAwait(false);
                        break;
                    default:
                        break;
                }
            }
        }

        private async Task RefreshCommandHandlerAsync()
        {
            switch (CurrentTab.Title.ToLower())
            {
                case "категории":
                    await LoadCategoryContentDataAsync(CurrentTab).ConfigureAwait(false);
                    break;
                case "авторы":
                    await LoadAuhorContentDataAsync(CurrentTab).ConfigureAwait(false);
                    break;
                default:
                    break;
            }
        }
    }
}
