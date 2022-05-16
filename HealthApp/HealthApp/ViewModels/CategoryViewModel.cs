using HealthApp.Common.Model;
using HealthApp.Common.Model.Helper;
using HealthApp.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthApp.ViewModels
{
    public class CategoryViewModel : BaseViewModel
    {
        public List<Category> Categories { get; set; }

        public CategoryViewModel()
        {
            LoadCategories();
        }

        private async void LoadCategories()
        {
            var categories = await GetCategoriesAsync();

            Categories = categories.ToList();
        }

        private async Task<List<Category>> GetCategoriesAsync()
        {
            string url = BaseUrl + ApiRoutes.GetCategories;

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
