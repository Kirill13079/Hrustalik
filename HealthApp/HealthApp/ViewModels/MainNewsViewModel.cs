using HealthApp.Common.Model;
using HealthApp.Common.Model.Helper;
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
        /// <summary>
        /// Получить экземпляр этого класса
        /// </summary>
        private static readonly MainNewsViewModel _instance = new MainNewsViewModel();
        public static MainNewsViewModel Instance => _instance;

        private ObservableRangeCollection<PopularTabModel> _tabPopularRecords;
        public ObservableRangeCollection<PopularTabModel> TabPopularRecords
        {
            get => _tabPopularRecords;
            set
            {
                _tabPopularRecords = value;
                OnPropertyChanged();
            }
        }

        private Record _hotNews;
        public Record HotRecord
        {
            get => _hotNews;
            set
            {
                _hotNews = value;
                OnPropertyChanged();
            }
        }

        public MainNewsViewModel()
        {
            TabPopularRecords = new ObservableRangeCollection<PopularTabModel>();
            HotRecord = new Record();

            _ = GetData();
        }

        private async Task GetData()
        {
            if (TabPopularRecords.Any())
            {
                TabPopularRecords.Clear();
            }

            for (int page = 0; page <= 3; page++)
            {
                TabPopularRecords.Add(new PopularTabModel
                {
                    Page = page
                });
            }

            foreach (var tab in TabPopularRecords)
            {
                await LoadContentData(tab).ConfigureAwait(false);
            }
        }

        private async Task LoadContentData(PopularTabModel tab, bool isRefreshing = false)
        {
            tab.HasError = false;

            try
            {
                if (tab.Records.Count == 0)
                {
                    tab.IsBusy = true;

                    var articles = await GetPopularsRecordAsync();

                    tab.Records.AddRange(articles);

                    tab.IsBusy = false;
                }
                else if (isRefreshing)
                {
                    tab.IsRefreshing = true;

                    var articles = await GetPopularsRecordAsync();

                    tab.Records.ReplaceRange(articles);
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

        private async Task<Record> GetHotRecordAsync()
        {
            string url = ApiRoutes.BaseUrl + ApiRoutes.GetHotRecord;

            var result = await ApiCaller.Get(url);

            if (!string.IsNullOrWhiteSpace(result))
            {
                var hot = JsonConvert.DeserializeObject<Record>(result);

                return hot;
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
                var populars = JsonConvert.DeserializeObject<List<Record>>(result);

                return populars.Take(1).ToList();
            }
            else
            {
                return null;
            }
        }
    }
}
