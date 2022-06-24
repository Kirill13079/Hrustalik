using HealthApp.Common.Model;
using HealthApp.Extensions;
using HealthApp.Helpers;
using HealthApp.Models;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace HealthApp.ViewModels.Main
{
    public partial class MainViewModel : BaseViewModel
    {
        private ObservableRangeCollection<TabModel> _tabCategoriesRecords = new ObservableRangeCollection<TabModel>();
        public ObservableRangeCollection<TabModel> TabCategoriesRecords
        {
            get => _tabCategoriesRecords;
            set
            {
                _tabCategoriesRecords = value;
                OnPropertyChanged();
            }
        }

        private TabModel _currentTab = new TabModel();
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
            await LoadCategoriesNewsViewContentData(CurrentTab, isRefreshing: true);
        });

        public ICommand ReloadCommand => new Command(async () =>
        {
            foreach (var tab in TabCategoriesRecords)
            {
                await LoadCategoriesNewsViewContentData(tab).ConfigureAwait(false);
            }
        });

        public ICommand SelectActiveTabCommand => new Command((obj) =>
        {
            CurrentTab = (TabModel)obj;
        });

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

                    _savedUserAuthors = AuthorsHelper.GetSavedUserAuthors();

                    var bookmarks = await ApiManager.GetBookmarksAsync();
                    var records = await ApiManager.GetCategoryRecordsAsync(tab.Page);

                    _ = records.RemoveAll(match: record => !_savedUserAuthors.EqualsHelper(record.Author));

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

                    tab.Records.AddRange(records);

                    tab.IsBusy = false;
                }
                else if (isRefreshing)
                {
                    tab.IsRefreshing = true;

                    _savedUserAuthors = AuthorsHelper.GetSavedUserAuthors();

                    var bookmarks = await ApiManager.GetBookmarksAsync();
                    var records = await ApiManager.GetCategoryRecordsAsync(tab.Page);

                    _ = records.RemoveAll(match: record => !_savedUserAuthors.EqualsHelper(record.Author));

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
