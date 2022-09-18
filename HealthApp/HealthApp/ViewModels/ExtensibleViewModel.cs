using HealthApp.Common.Model;
using HealthApp.Extensions;
using HealthApp.Helpers;
using HealthApp.Models;
using HealthApp.Utils;
using HealthApp.ViewModels.Base;
using HealthApp.ViewModels.Data;
using MvvmHelpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HealthApp.ViewModels
{
    public class ExtensibleViewModel : ViewBaseModel
    {
        private readonly ObservableRangeCollection<ExtensibleModel> _cachingTabExtensibleItems = new ObservableRangeCollection<ExtensibleModel>();

        private ObservableRangeCollection<ExtensibleModel> _tabExtensibleItems = new ObservableRangeCollection<ExtensibleModel>();
        public ObservableRangeCollection<ExtensibleModel> TabExtensibleItems
        {
            get => _tabExtensibleItems;
            set
            {
                _tabExtensibleItems = value;
                OnPropertyChanged();
            }
        }

        private ExtensibleModel _currentTabExtensibleItem = new ExtensibleModel();
        public ExtensibleModel CurrentTabExtensibleItem
        {
            get => _currentTabExtensibleItem;
            set
            {
                _currentTabExtensibleItem = value;
                OnPropertyChanged();
            }
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();

                CurrentTabExtensibleItem.ExtensibleItems = FilteredItems(SearchText);
            }
        }

        public ExtensibleViewModel()
        {
            ReloadCommand = new Command(async () => await ReloadCommandHandlerAsync());

            _ = GetDataAsync().ConfigureAwait(false);
        }

        private async Task GetDataAsync()
        {
            if (TabExtensibleItems.Any())
            {
                TabExtensibleItems.Clear();
            }

            SetExtensibleItems();

            CurrentTabExtensibleItem = TabExtensibleItems[0];

            await ReloadCommandHandlerAsync();
        }

        private async Task LoadAuthorContentDataAsync(ExtensibleModel tab)
        {
            tab.HasError = false;

            tab.CurrentStateTab = LayoutState.Loading;

            try
            {
                ObservableRangeCollection<ExtensibleObject> authors = await GetAuthorsAsync();

                if (tab.ExtensibleItems.Count == 0)
                {
                    tab.ExtensibleItems.AddRange(authors);
                }
                else
                {
                    tab.ExtensibleItems.ReplaceRange(authors);
                }

                tab.CurrentStateTab = LayoutState.Success;
            }
            catch
            {
                tab.HasError = true;

                tab.CurrentStateTab = LayoutState.Error;
            }
        }

        private async Task LoadCategoryContentDataAsync(ExtensibleModel tab)
        {
            tab.HasError = false;

            tab.CurrentStateTab = LayoutState.Loading;

            try
            {
                ObservableRangeCollection<ExtensibleObject> categories = await GetCategoriesAsync();

                if (tab.ExtensibleItems.Count == 0)
                {
                    tab.ExtensibleItems.AddRange(categories);
                }
                else
                {
                    tab.ExtensibleItems.ReplaceRange(categories);
                }

                tab.CurrentStateTab = LayoutState.Success;
            }
            catch
            {
                tab.HasError = true;

                tab.CurrentStateTab = LayoutState.Error;
            }
        }

        private ObservableRangeCollection<ExtensibleObject> FilteredItems(string searchText)
        {
            ObservableRangeCollection<ExtensibleObject> filter = new ObservableRangeCollection<ExtensibleObject>();

            MainThread.BeginInvokeOnMainThread(() =>
            {
                AppEnum.App currentTab = CurrentTabExtensibleItem.Subtitle.ConvertToEnum<AppEnum.App>();

                ExtensibleModel cachingItems = new ExtensibleModel();

                switch (currentTab)
                {
                    case AppEnum.App.Categories:
                        cachingItems = _cachingTabExtensibleItems.FirstOrDefault(tab => tab.Subtitle == AppEnum.App.Categories.ConvertToString());
                        break;
                    case AppEnum.App.Authors:
                        cachingItems = _cachingTabExtensibleItems.FirstOrDefault(tab => tab.Subtitle == AppEnum.App.Authors.ConvertToString());
                        break;
                    default:
                        break;
                }

                for (int index = 0; index < cachingItems.ExtensibleItems.Count; index++)
                {
                    ExtensibleObject item = cachingItems.ExtensibleItems[index];

                    if (item.Category.Name.ToLower().Contains(searchText.ToLower()))
                    {
                        filter.Add(item);
                    }
                }
            });

            return filter;
        }

        private async Task<ObservableRangeCollection<ExtensibleObject>> GetCategoriesAsync()
        {
            List<Category> categories = await ApiManagerService.GetCategoriesAsync();
            List<Category> savedUserCategories = await CategoriesHelper.GetSavedUserCategoriesAsync();

            ObservableRangeCollection<ExtensibleObject> articles = new ObservableRangeCollection<ExtensibleObject>();

            foreach (Category category in categories)
            {
                ExtensibleObject article = new ExtensibleObject
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

            return articles;
        }

        private async Task<ObservableRangeCollection<ExtensibleObject>> GetAuthorsAsync()
        {
            List<Author> authors = await ApiManagerService.GetAuthorsAsync();
            List<Author> savedUserAuthors = await AuthorsHelper.GetSavedUserAuthorsAsync();

            ObservableRangeCollection<ExtensibleObject> articles = new ObservableRangeCollection<ExtensibleObject>();

            foreach (Author author in authors)
            {
                ExtensibleObject article = new ExtensibleObject
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

            return articles;
        }

        private void SetExtensibleItems()
        {
            TabExtensibleItems = new ObservableRangeCollection<ExtensibleModel>()
            {
                {
                    new ExtensibleModel
                    {
                        Title = AppEnum.App.Categories.DisplayName(),
                        Subtitle = AppEnum.App.Categories.ConvertToString()
                    }
                },
                {
                    new ExtensibleModel
                    {
                        Title = AppEnum.App.Authors.DisplayName(),
                        Subtitle = AppEnum.App.Authors.ConvertToString()
                    }
                }
            };
        }

        private async Task ReloadCommandHandlerAsync()
        {
            if (_cachingTabExtensibleItems.Any())
            {
                _cachingTabExtensibleItems.Clear();
            }

            foreach (ExtensibleModel tab in TabExtensibleItems)
            {
                AppEnum.App currentTab = tab.Subtitle.ConvertToEnum<AppEnum.App>();

                switch (currentTab)
                {
                    case AppEnum.App.Categories:
                        await LoadCategoryContentDataAsync(tab).ConfigureAwait(false);
                        break;
                    case AppEnum.App.Authors:
                        await LoadAuthorContentDataAsync(tab).ConfigureAwait(false);
                        break;
                    default:
                        break;
                }

                _cachingTabExtensibleItems.Add(new ExtensibleModel()
                {
                    ExtensibleItems = tab.ExtensibleItems,
                    Subtitle = tab.Subtitle
                });
            }
        }
    }
}
