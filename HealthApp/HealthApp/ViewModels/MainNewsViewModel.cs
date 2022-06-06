using HealthApp.Common.Model;
using HealthApp.Common.Model.Helper;
using HealthApp.Models;
using HealthApp.Service;
using MvvmHelpers;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthApp.ViewModels
{
    public class MainNewsViewModel : BaseViewModel
    {
        private static readonly MainNewsViewModel _instance = new MainNewsViewModel();
        public static MainNewsViewModel Instance => _instance;

        private int _pageSize = 10;
        private int _takeRecord = 1;
        private int _skipRecords = 0;

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
            MainTabModel = new MainTabModel();

            _ = GetData();
        }

        private async Task GetData()
        {
            if (MainTabModel.SubTabModel.Any())
            {
                MainTabModel.SubTabModel.Clear();
            }

            for (int page = 0; page <= _pageSize; page++)
            {
                MainTabModel.SubTabModel.Add(new TabModel 
                { 
                    Page = page 
                });
            }

            MainTabModel.HotRecord = await GetHotRecordAsync();

            var records = await GetRecordsAsync();

            foreach (var record in records)
            {
                MainTabModel.Records.Add(record);
            }

            _skipRecords = 0;

            foreach (var tab in MainTabModel.SubTabModel)
            {
                _skipRecords++;

                await LoadPopularContentData(tab).ConfigureAwait(false);
            }
        }

        private async Task LoadPopularContentData(TabModel tab, bool isRefreshing = false)
        {
            tab.HasError = false;

            try
            {
                if (tab.Records.Count == 0)
                {
                    tab.IsBusy = true;

                    var records = await GetPopularsRecordAsync();

                    tab.Records.AddRange(records);

                    tab.IsBusy = false;
                }
                else if (isRefreshing)
                {
                    tab.IsRefreshing = true;

                    var records = await GetPopularsRecordAsync();

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

        private async Task<List<Record>> GetRecordsAsync()
        {
            string url = ApiRoutes.BaseUrl + ApiRoutes.GetRecords;

            var result = await ApiCaller.Get(url);

            if (!string.IsNullOrWhiteSpace(result))
            {
                var records = JsonConvert.DeserializeObject<List<Record>>(result);

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

        private async Task<Record> GetHotRecordAsync()
        {
            string url = ApiRoutes.BaseUrl + ApiRoutes.GetHotRecord;

            var result = await ApiCaller.Get(url);

            if (!string.IsNullOrWhiteSpace(result))
            {
                var record = JsonConvert.DeserializeObject<Record>(result);

                record.Image = $"{ApiRoutes.BaseUrl}/RecordImages/{record.Image}";
                record.Author.Logo = $"{ApiRoutes.BaseUrl}/AuthorImages/{record.Author.Logo}";

                return record;
            }
            else
            {
                return null;
            }
        }

        private async Task<List<Record>> GetPopularsRecordAsync()
        {
            string url = ApiRoutes.BaseUrl + ApiRoutes.GetPopularRecords;

            var result = await ApiCaller.Get(url);

            if (!string.IsNullOrWhiteSpace(result))
            {
                var records = JsonConvert.DeserializeObject<List<Record>>(result);

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
