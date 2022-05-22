using HealthApp.Common.Model;
using HealthApp.Common.Model.Helper;
using HealthApp.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HealthApp.ViewModels.Authors
{
    [QueryProperty("Parameter", "parameter")]
    [QueryProperty("Title", "title")]
    public class AuthorInfoViewModel : BaseViewModel
    {
        public Author CurrentAuthor { get; set; }

        public List<Record> AuthorRecords { get; set; }

        public ICommand OpenLinkCommand => new Command(async (obj) =>
        {
            await Browser.OpenAsync((string)obj);
        });

        private string parameter;
        public string Parameter
        {
            get { return parameter; }
            set
            {
                parameter = Uri.UnescapeDataString(value);
                CurrentAuthor = JsonConvert.DeserializeObject<Author>(parameter);
                LoadRecords();
            }
        }

        public async void LoadRecords()
        {
            await Task.Delay(100);

            var records = await GetAuthorRecordsAsync();

            AuthorRecords = records;
        }

        private async Task<List<Record>> GetAuthorRecordsAsync()
        {
            string url = $"{BaseUrl}{ApiRoutes.GetAuthorRecords}?id={CurrentAuthor.Id}";

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
