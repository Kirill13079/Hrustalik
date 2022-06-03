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
        /// <summary>
        /// Получить экземпляр этого класса
        /// </summary>
        private static readonly MainNewsViewModel _instance = new MainNewsViewModel();
        public static MainNewsViewModel Instance => _instance;

        private ObservableRangeCollection<TabModel> _tabItems;
        public ObservableRangeCollection<TabModel> TabItems
        {
            get => _tabItems;
            set
            {
                _tabItems = value;
                OnPropertyChanged();
            }
        }

        private TabModel _currentTab;
        public TabModel CurrentTab
        {
            get => _currentTab;
            set
            {
                _currentTab = value;
                OnPropertyChanged();
            }
        }

        private Record _hotNews;
        public Record HotNews
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
            TabItems = new ObservableRangeCollection<TabModel>();
            HotNews = new Record();

            _ = GetData();
        }

        private async Task GetData()
        {
            if (TabItems.Any())
            {
                TabItems.Clear();
            }

            for (int page = 0; page <= 10; page++)
            {
                TabItems.Add(new TabModel
                {
                    Page = page
                });
            }

            foreach (var tab in TabItems)
            {
                await LoadContentData(tab).ConfigureAwait(false);
            }
        }

        private async Task LoadContentData(TabModel tab, bool isRefreshing = false)
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

                return populars;
            }
            else
            {
                return null;
            }
        }
    }
}
