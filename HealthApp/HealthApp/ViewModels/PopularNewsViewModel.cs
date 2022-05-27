using HealthApp.Common.Model;
using HealthApp.Common.Model.Helper;
using HealthApp.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace HealthApp.ViewModels
{
    public class PopularNewsViewModel : BaseViewModel
    {
        public List<Record> Records { get; set; } = new List<Record>();

        public ICommand SelectRecordCommand => new Command((obj) =>
        {
            NavigateTo("record", (Record)obj);
        });

        public ICommand SelectAuthorCommand => new Command((obj) =>
        {
            NavigateTo("authorInfo", (Author)obj);
        });

        public PopularNewsViewModel()
        {
            LoadRecords();
        }

        private async void LoadRecords()
        {
            var records = await GetPopularRecordsAsync();

            Records = records.ToList();
        }

        private async Task<List<Record>> GetPopularRecordsAsync()
        {
            string url = BaseUrl + ApiRoutes.GetPopularRecords;

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
