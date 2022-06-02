using HealthApp.Common.Model;
using HealthApp.Common.Model.Helper;
using HealthApp.Models;
using HealthApp.Service;
using MvvmHelpers;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace HealthApp.ViewModels
{
    public class NewsViewModel : BaseViewModel
    {
        /// <summary>
        /// Получить экземпляр этого класса
        /// </summary>
        private static readonly NewsViewModel _instance = new NewsViewModel();
        public static NewsViewModel Instance => _instance;

        /// <summary>
        /// Колекция доступных вкладок
        /// </summary>
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

        /// <summary>
        /// Текущая вкладка
        /// </summary>
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

        /// <summary>
        /// Команада обновления контента на вкладке
        /// </summary>
        public ICommand RefreshCommand => new Command(async () => 
        {
            await LoadContentData(CurrentTab, isRefreshing: true);
        });

        /// <summary>
        /// Команда обновление всей информации
        /// </summary>
        public ICommand ReloadCommand => new Command(async () => 
        {
            foreach (var tab in TabItems)
            {
                await LoadContentData(tab).ConfigureAwait(false);
            }
        });

        /// <summary>
        /// Команда для активации активной вкладки
        /// </summary>
        public ICommand SelectActiveTabCommand => new Command((obj) => 
        {
            CurrentTab = (TabModel)obj;
        });

        /// <summary>
        /// Конструктор
        /// </summary>
        public NewsViewModel()
        {
            TabItems = new ObservableRangeCollection<TabModel>();
            CurrentTab = new TabModel();

            _ = GetData();
        }

        /// <summary>
        /// Метод загрузки всей информации
        /// вкладки, контент на вкладке
        /// </summary>
        /// <returns></returns>
        public async Task GetData()
        {
            if (TabItems.Any())
            {
                TabItems.Clear();
            }

            var categories = await GetCategoriesAsync();

            var tabItems = new ObservableRangeCollection<TabModel>();

            foreach (var category in categories)
            {
                tabItems.Add(new TabModel
                {
                    Title = category.Name,
                    CategoryId = category.Id
                });
            }

            TabItems = tabItems;
            CurrentTab = TabItems[0];

            foreach (var tab in TabItems)
            {
                await LoadContentData(tab).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Метод загрузки контента
        /// </summary>
        /// <param name="tab">Активная вкладка</param>
        /// <param name="isRefreshing">Если обновляем свайпом</param>
        /// <returns></returns>
        private async Task LoadContentData(TabModel tab, bool isRefreshing = false)
        {
            tab.HasError = false;

            try
            {
                if (tab.Records.Count == 0)
                {
                    tab.IsBusy = true;

                    var articles = await GetCategoryRecordsAsync(tab.CategoryId);

                    tab.Records.AddRange(articles);

                    tab.IsBusy = false;
                }
                else if (isRefreshing)
                {
                    tab.IsRefreshing = true;

                    var articles = await GetCategoryRecordsAsync(tab.CategoryId);

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

        /// <summary>
        /// Получить список новостей из категории
        /// </summary>
        /// <param name="categoryId">Категория</param>
        /// <returns></returns>
        private async Task<List<Record>> GetCategoryRecordsAsync(int categoryId)
        {
            string url = $"{ApiRoutes.BaseUrl}{ApiRoutes.GetCategoryRecords}?id={categoryId}";

            var result = await ApiCaller.Get(url);

            if (!string.IsNullOrWhiteSpace(result))
            {
                var records = JsonConvert.DeserializeObject<List<Record>>(result);

                records.ForEach((record) =>
                {
                    record.Image = $"{ApiRoutes.BaseUrl}/RecordImages/{record.Image}";
                    record.Author.Logo = $"{ApiRoutes.BaseUrl}/AuthorImages/{record.Author.Logo}";
                });

                return records;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Получить список всех категорий
        /// </summary>
        /// <returns></returns>
        private async Task<List<Category>> GetCategoriesAsync()
        {
            string url = ApiRoutes.BaseUrl + ApiRoutes.GetCategories;

            var result = await ApiCaller.Get(url);

            if (!string.IsNullOrWhiteSpace(result))
            {
                var categories = JsonConvert.DeserializeObject<List<Category>>(result);

                return categories;
            }
            else
            {
                return null;
            }
        }
    }
}
