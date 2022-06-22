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

namespace HealthApp.ViewModels
{
    public class MainNewsViewModel : BaseViewModel
    {
        private int _pageSize = 10;
        private int _takeRecord = 1;
        private int _skipRecords = 0;

        private List<Author> _savedUserAuthors;
        private List<Category> _savedUserCategories;

        private static MainNewsViewModel _instance;
        public static MainNewsViewModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MainNewsViewModel();
                }

                return _instance;
            }
        }

        private MainTabModel _mainTabModel;
        public MainTabModel MainTabModel
        {
            get => _mainTabModel;
            set
            {
                _mainTabModel = value;
                OnPropertyChanged();
            }
        }

        public MainNewsViewModel()
        {
            _savedUserAuthors = new List<Author>();
            _savedUserCategories = new List<Category>();

            MainTabModel = new MainTabModel();

            _ = GetData();
        }

        public async Task GetData()
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

            MainTabModel.HotRecord = await GetHotRecordAsync();

            await LoadRecordsContentData().ConfigureAwait(false);

            _skipRecords = 0;

            foreach (var tab in MainTabModel.SubTabModel)
            {
                _skipRecords++;

                await LoadPopularRecordsContentData(tab).ConfigureAwait(false);
            }
        }

        private async Task LoadRecordsContentData()
        {
            var records = await GetRecordsAsync();

            _savedUserAuthors = AuthorsHelper.GetSavedUserAuthors();
            _savedUserCategories = CategoriesHelper.GetSavedUserCategories();

            Predicate<Category> removedCategories = (Category category) => !_savedUserCategories.EqualsHelper(category);
            Predicate<Author> removedAuthors = (Author author) => !_savedUserAuthors.EqualsHelper(author);

            records.RemoveAll(record => removedCategories(record.Category));
            records.RemoveAll(record => removedAuthors(record.Author));

            MainTabModel.Records.AddRange(records);
        }

        private async Task LoadPopularRecordsContentData(TabModel tab, bool isRefreshing = false)
        {
            tab.HasError = false;

            try
            {
                if (tab.Records.Count == 0)
                {
                    tab.IsBusy = true;

                    var records = await GetPopularsRecordAsync();

                    Predicate<Category> removedCategories = (Category category) => !_savedUserCategories.EqualsHelper(category);
                    Predicate<Author> removedAuthors = (Author author) => !_savedUserAuthors.EqualsHelper(author);

                    records.RemoveAll(record => removedCategories(record.Category));
                    records.RemoveAll(record => removedAuthors(record.Author));

                    tab.Records.AddRange(records);

                    tab.IsBusy = false;
                }
                else if (isRefreshing)
                {
                    tab.IsRefreshing = true;

                    var records = await GetPopularsRecordAsync();

                    Predicate<Category> removedCategories = (Category category) => !_savedUserCategories.EqualsHelper(category);
                    Predicate<Author> removedAuthors = (Author author) => !_savedUserAuthors.EqualsHelper(author);

                    records.RemoveAll(record => removedCategories(record.Category));
                    records.RemoveAll(record => removedAuthors(record.Author));

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

        private async Task<List<RecordModel>> GetRecordsAsync()
        {
            string url = ApiRoutes.BaseUrl + ApiRoutes.GetRecords;

            var result = await ApiCaller.Get(url);

            if (!string.IsNullOrWhiteSpace(result))
            {
                var records = JsonConvert.DeserializeObject<List<RecordModel>>(result);

                records.ForEach((record) =>
                {
                    record.Image = $"{ApiRoutes.BaseUrl}/RecordImages/{record.Image}";
                    record.Author.Logo = $"{ApiRoutes.BaseUrl}/AuthorImages/{record.Author.Logo}";
                });

                return records.ToList();
            }
            else
            {
                return null;
            }
        }

        private async Task<RecordModel> GetHotRecordAsync()
        {
            string url = ApiRoutes.BaseUrl + ApiRoutes.GetHotRecord;

            var result = await ApiCaller.Get(url);

            if (!string.IsNullOrWhiteSpace(result))
            {
                var record = JsonConvert.DeserializeObject<RecordModel>(result);

                record.Image = $"{ApiRoutes.BaseUrl}/RecordImages/{record.Image}";
                record.Author.Logo = $"{ApiRoutes.BaseUrl}/AuthorImages/{record.Author.Logo}";

                return record;
            }
            else
            {
                return null;
            }
        }

        private async Task<List<RecordModel>> GetPopularsRecordAsync()
        {
            string url = ApiRoutes.BaseUrl + ApiRoutes.GetPopularRecords;

            var result = await ApiCaller.Get(url);

            if (!string.IsNullOrWhiteSpace(result))
            {
                var records = JsonConvert.DeserializeObject<List<RecordModel>>(result);

                records.ForEach((record) =>
                {
                    record.Image = $"{ApiRoutes.BaseUrl}/RecordImages/{record.Image}";
                    record.Author.Logo = $"{ApiRoutes.BaseUrl}/AuthorImages/{record.Author.Logo}";
                });

                return records.Skip(_skipRecords).Take(_takeRecord).ToList();
            }
            else
            {
                return null;
            }
        }
    }
}
