using HealthApp.Common.Model;
using HealthApp.Common.Model.Helper;
using HealthApp.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HealthApp.ViewModels.Categories
{
    [QueryProperty("Parameter", "parameter")]
    [QueryProperty("Title", "title")]
    public class CategoryInfoViewModel : BaseViewModel
    {
        public Category CurrentCategory { get; set; }

        public List<Record> CategoryRecords { get; set; }

        private string parameter;
        public string Parameter
        {
            get { return parameter; }
            set
            {
                parameter = Uri.UnescapeDataString(value);
                CurrentCategory = JsonConvert.DeserializeObject<Category>(parameter);

                LoadRecords();
            }
        }

        public async void LoadRecords()
        {
            await Task.Delay(100);

            var records = await GetCategoryRecordsAsync();

            CategoryRecords = records;
        }

        private async Task<List<Record>> GetCategoryRecordsAsync()
        {
            string url = $"{BaseUrl}{ApiRoutes.GetCategoryRecords}?id={CurrentCategory.Id}";

            var result = await ApiCaller.Get(url);

            if (!string.IsNullOrWhiteSpace(result))
            {
                var records = JsonConvert.DeserializeObject<List<Record>>(result);

                records.ForEach((record) =>
                {
                    record.Image = $"{BaseUrl}/RecordImages/{record.Image}";
                    record.Author.Logo = $"{BaseUrl}/AuthorImages/{record.Author.Logo}";
                });

                return records;
            }
            else
            {
                return null;
            }
        }
    }
}
