﻿using System.Linq;
using System.Threading.Tasks;
using HealthApp.Extensions;
using HealthApp.Helpers;
using HealthApp.Models;
using HealthApp.ViewModels.Base;
using HealthApp.ViewModels.Data;
using MvvmHelpers;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace HealthApp.ViewModels
{
    public class CategoryViewModel : ViewBaseModel
    {
        private ObservableRangeCollection<TabModel> _categoriesTab = new ObservableRangeCollection<TabModel>();
        public ObservableRangeCollection<TabModel> CategoriesTab
        {
            get => _categoriesTab;
            set
            {
                _categoriesTab = value;
                OnPropertyChanged();
            }
        }

        private TabModel _currentCategoryTab = new TabModel();
        public TabModel CurrentCategoryTab
        {
            get => _currentCategoryTab;
            set
            {
                _currentCategoryTab = value;
                OnPropertyChanged();
            }
        }

        public CategoryViewModel()
        {
            RefreshCommand = new Command(async () =>
            {
                await LoadCategoryContentDataAsync(CurrentCategoryTab, true);
            });

            ReloadCommand = new Command(async () =>
            {
                foreach (var tab in CategoriesTab)
                { 
                    await LoadCategoryContentDataAsync(tab).ConfigureAwait(false); 
                }
            });

            LikeRecordCommand = new Command<RecordViewModel>(likedRecord => 
            {
                bool isLikedRecord(RecordViewModel record) => record.Equals(likedRecord);

                foreach (var tab in CategoriesTab)
                {
                    if (tab.Records.Count > 0)
                    {
                        var categoryRecord = tab.Records.FirstOrDefault(predicate: record => isLikedRecord(record));

                        if (categoryRecord != null)
                        {
                            categoryRecord.IsBookmark = !categoryRecord.IsBookmark;
                        }
                    }
                }
            });

            _ = GetDataAsync().ConfigureAwait(false);
        }

        public async Task GetDataAsync()
        {
            CurrentState = LayoutState.Loading;

            await GetDataCategoriesAsync();

            CurrentState = LayoutState.None;
        }

        private async Task GetDataCategoriesAsync()
        {
            if (CategoriesTab.Any())
            {
                CategoriesTab.Clear();
            }

            var categories = await ApiManager.GetCategoriesAsync();
            var tabItems = new ObservableRangeCollection<TabModel>();

            if (categories != null)
            {
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

                CategoriesTab = tabItems;
                CurrentCategoryTab = CategoriesTab[0];

                foreach (var tab in CategoriesTab)
                {
                    await LoadCategoryContentDataAsync(tab).ConfigureAwait(false);
                }
            }
            else
            {
                tabItems.Add(new TabModel
                {
                    Title = "",
                    Page = 0
                });
            }
        }

        private async Task LoadCategoryContentDataAsync(TabModel tab, bool isRefreshing = false)
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