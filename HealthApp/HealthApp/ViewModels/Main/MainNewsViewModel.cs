using HealthApp.Extensions;
using HealthApp.Helpers;
using HealthApp.Models;
using MvvmHelpers;
using System.Linq;
using System.Threading.Tasks;

namespace HealthApp.ViewModels.Main
{
    public partial class MainViewModel : BaseViewModel
    {
        private int _pageSize = 2;
        private int _takeRecord = 1;
        private int _skipRecords = 0;

        private MainTabModel _mainTabModel = new MainTabModel();
        public MainTabModel MainTabModel
        {
            get => _mainTabModel;
            set
            {
                _mainTabModel = value;
                OnPropertyChanged();
            }
        }

        private async Task LoadMainNewsRecordsContentData()
        {
            MainTabModel.HasError = false;

            try
            {
                MainTabModel.IsBusy = true;

                var records = await ApiManager.GetRecordsAsync();
                var bookmarks = await ApiManager.GetBookmarksAsync();

                _savedUserAuthors = AuthorsHelper.GetSavedUserAuthors();
                _savedUserCategories = CategoriesHelper.GetSavedUserCategories();

                records.RemoveAll(record => !_savedUserCategories.EqualsHelper(record.Category));
                records.RemoveAll(record => !_savedUserAuthors.EqualsHelper(record.Author));

                if (bookmarks != null)
                {
                    foreach (var record in records)
                    {
                        if (bookmarks.Where(boomark => boomark.Record.Id == record.Id).Any())
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

        private async Task LoadPopularMainNewsRecordsContentData(TabModel tab, bool isRefreshing = false)
        {
            tab.HasError = false;

            try
            {
                if (tab.Records.Count == 0)
                {
                    tab.IsBusy = true;

                    var records = await ApiManager.GetPopularsRecordsAsync(_skipRecords, _takeRecord);

                    records.RemoveAll(record => !_savedUserCategories.EqualsHelper(record.Category));
                    records.RemoveAll(record => !_savedUserAuthors.EqualsHelper(record.Author));

                    tab.Records.AddRange(records);

                    tab.IsBusy = false;
                }
                else if (isRefreshing)
                {
                    tab.IsRefreshing = true;

                    var records = await ApiManager.GetPopularsRecordsAsync(_skipRecords, _takeRecord);

                    records.RemoveAll(record => !_savedUserCategories.EqualsHelper(record.Category));
                    records.RemoveAll(record => !_savedUserAuthors.EqualsHelper(record.Author));

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
    }
}
