﻿using HealthApp.Common.Model;
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
    public class CategoryViewModel : BaseViewModel
    {
        /// <summary>
        /// Получить экземпляр этого класса
        /// </summary>
        private static readonly CategoryViewModel _instance = new CategoryViewModel();
        public static CategoryViewModel Instance => _instance;

        /// <summary>
        /// Колекция доступных вкладок
        /// </summary>
        private ObservableRangeCollection<TabModel> _tabCategoriesRecords;
        public ObservableRangeCollection<TabModel> TabCategoriesRecords
        { 
            get => _tabCategoriesRecords; 
            set 
            { 
                _tabCategoriesRecords = value; 
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
            foreach (var tab in TabCategoriesRecords)
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
        public CategoryViewModel()
        {
            TabCategoriesRecords = new ObservableRangeCollection<TabModel>();
            CurrentTab = new TabModel();

            _ = GetData();
        }

        /// <summary>
        /// Метод загрузки всей информации
        /// </summary>
        /// <returns></returns>
        public async Task GetData()
        {
            if (TabCategoriesRecords.Any())
            {
                TabCategoriesRecords.Clear();
            }

            var categories = await GetCategoriesAsync();

            var tabItems = new ObservableRangeCollection<TabModel>();

            foreach (var category in categories)
            {
                tabItems.Add(new TabModel
                {
                    Title = category.Name,
                    Page = category.Id
                });
            }

            TabCategoriesRecords = tabItems;
            CurrentTab = TabCategoriesRecords[0];

            foreach (var tab in TabCategoriesRecords)
            {
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

                    var articles = await GetCategoryRecordsAsync(tab.Page);

                    tab.Records.AddRange(articles);

                    tab.IsBusy = false;
                }
                else if (isRefreshing)
                {
                    tab.IsRefreshing = true;

                    var articles = await GetCategoryRecordsAsync(tab.Page);

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