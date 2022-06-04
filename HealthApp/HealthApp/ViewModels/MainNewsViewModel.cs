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

        private int _pageSize = 10;
        private int _takeRecord = 1;
        private int _skipRecords = 0;

        /// <summary>
        /// Колекция доступных вкладок
        /// </summary>
        private ObservableRangeCollection<TabModel> _tabPopularRecords;
        public ObservableRangeCollection<TabModel> TabPopularRecords
        {
            get => _tabPopularRecords;
            set
            {
                _tabPopularRecords = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Самая важная новость
        /// </summary>
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

        /// <summary>
        /// Конструктор
        /// </summary>
        public MainNewsViewModel()
        {
            TabPopularRecords = new ObservableRangeCollection<TabModel>();
            HotRecord = new Record();

            _ = GetData();
        }

        /// <summary>
        /// Метод загрузки всей информации
        /// </summary>
        /// <returns></returns>
        private async Task GetData()
        {
            if (TabPopularRecords.Any())
            {
                TabPopularRecords.Clear();
            }

            for (int page = 0; page <= _pageSize; page++)
            {
                TabPopularRecords.Add(new TabModel
                {
                    Page = page
                });
            }

            _skipRecords = 0;

            foreach (var tab in TabPopularRecords)
            {
                _skipRecords++;

                await LoadContentData(tab).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Метод загрузки контента
        /// </summary>
        /// <param name="tab">Активная вкладка</param>
        /// <param name="isRefreshing">Если обновляем контент</param>
        /// <returns></returns>
        private async Task LoadContentData(TabModel tab, bool isRefreshing = false)
        {
            tab.HasError = false;

            try
            {
                if (tab.Records.Count == 0)
                {
                    tab.IsBusy = true;

                    var articles = await GetPopularsRecordAsync(_skipRecords);

                    tab.Records.AddRange(articles);

                    tab.IsBusy = false;
                }
                else if (isRefreshing)
                {
                    tab.IsRefreshing = true;

                    var articles = await GetPopularsRecordAsync(_skipRecords);

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

        private async Task<List<Record>> GetPopularsRecordAsync(int skipRecords)
        {
            string url = ApiRoutes.BaseUrl + ApiRoutes.GetPopularRecords;

            var result = await ApiCaller.Get(url);

            if (!string.IsNullOrWhiteSpace(result))
            {
                var records = JsonConvert.DeserializeObject<List<Record>>(result);

                records.ForEach((record) =>
                {
                    record.Image = $"{ApiRoutes.BaseUrl}/RecordImages/{record.Image}";
                });

                return records.Skip(skipRecords).Take(_takeRecord).ToList();
            }
            else
            {
                return null;
            }
        }
    }
}
