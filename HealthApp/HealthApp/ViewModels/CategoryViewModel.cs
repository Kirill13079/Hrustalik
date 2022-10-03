using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HealthApp.Common.Model;
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
        #region private

        private ObservableRangeCollection<TabModel> _categoriesTab = new ObservableRangeCollection<TabModel>();

        private TabModel _currentCategoryTab = new TabModel();

        #endregion

        public CategoryViewModel()
        {
            RefreshCommand = new Command(async () => await RefreshCommandHandlerAsync());
            ReloadCommand = new Command(async () => await ReloadCommandHandlerAsync());
            LikeRecordCommand = new Command<RecordViewModel>(likedRecord => LikeRecordCommandHandler(likedRecord));

            _ = GetDataAsync().ConfigureAwait(false);
        }

        #region properties

        public ObservableRangeCollection<TabModel> CategoriesTab
        {
            get => _categoriesTab;
            set
            {
                _categoriesTab = value;
                OnPropertyChanged();
            }
        }

        public TabModel CurrentCategoryTab
        {
            get => _currentCategoryTab;
            set
            {
                _currentCategoryTab = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region public methods

        public async Task GetDataAsync()
        {
            await GetDataCategoriesAsync();
        }

        #endregion

        #region private methods

        private async Task GetDataCategoriesAsync()
        {
            if (CategoriesTab.Any())
            {
                CategoriesTab.Clear();
            }

            List<Category> categories = await ApiManagerService.GetCategoriesAsync();

            ObservableRangeCollection<TabModel> tabItems = new ObservableRangeCollection<TabModel>();

            if (categories != null)
            {
                List<Category> savedUserCategories = await CategoriesHelper.GetSavedUserCategoriesAsync();

                _ = categories.RemoveAll(match: category => !savedUserCategories.EqualsHelper(category));

                if (categories.Count > 0)
                {
                    foreach (Category category in categories)
                    {
                        tabItems.Add(new TabModel
                        {
                            Title = category.Name,
                            Page = category.Id
                        });
                    }

                    CategoriesTab = tabItems;

                    CurrentCategoryTab = CategoriesTab[0];

                    foreach (TabModel tab in CategoriesTab)
                    {
                        await LoadCategoryContentDataAsync(tab).ConfigureAwait(false);
                    }
                }
            }
        }

        private async Task LoadCategoryContentDataAsync(TabModel tab, bool isRefreshing = false)
        {
            tab.CurrentStateTab = LayoutState.Loading;

            try
            {
                List<RecordViewModel> records = await GetCategories(tab);

                if (tab.Records.Count == 0)
                {
                    tab.Records.AddRange(records);
                }
                else if (isRefreshing)
                {
                    tab.IsRefreshing = true;

                    tab.Records.ReplaceRange(records);

                    tab.IsRefreshing = false;
                }

                tab.CurrentStateTab = tab.Records.Count == 0 ? LayoutState.Empty : LayoutState.Success;
            }
            catch
            {
                tab.CurrentStateTab = LayoutState.Error;
            }
            finally
            {
                tab.IsRefreshing = false;
            }
        }

        private async Task<List<RecordViewModel>> GetCategories(TabModel tab)
        {
            List<Author> savedUserAuthors = await AuthorsHelper.GetSavedUserAuthorsAsync();
            List<Bookmark> bookmarks = await ApiManagerService.GetBookmarksAsync();
            List<RecordViewModel> records = await ApiManagerService.GetCategoryRecordsAsync(categoryId: tab.Page);

            if (savedUserAuthors.Any())
            {
                _ = records.RemoveAll(match: record => !savedUserAuthors.EqualsHelper(record.Author));
            }

            if (bookmarks != null)
            {
                foreach (RecordViewModel record in records)
                {
                    if (bookmarks.Any(predicate: boomark => boomark.Record.Id == record.Id))
                    {
                        record.IsBookmark = true;
                    }
                }
            }

            return records;
        }

        #endregion

        #region command handlers

        private async Task RefreshCommandHandlerAsync()
        {
            await LoadCategoryContentDataAsync(CurrentCategoryTab, isRefreshing: true);
        }

        private async Task ReloadCommandHandlerAsync()
        {
            foreach (TabModel tab in CategoriesTab)
            {
                await LoadCategoryContentDataAsync(tab).ConfigureAwait(false);
            }
        }

        private void LikeRecordCommandHandler(RecordViewModel likedRecord)
        {
            bool isLikedRecord(RecordViewModel record)
            {
                return record.Equals(likedRecord);
            }

            foreach (TabModel tab in CategoriesTab)
            {
                if (tab.Records.Count > 0)
                {
                    RecordViewModel categoryRecord = tab.Records.FirstOrDefault(predicate: record => isLikedRecord(record));

                    if (categoryRecord != null)
                    {
                        categoryRecord.IsBookmark = !categoryRecord.IsBookmark;
                    }
                }
            }
        }

        #endregion
    }
}
