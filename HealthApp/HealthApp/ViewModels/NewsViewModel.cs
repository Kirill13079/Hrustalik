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
    public class NewsViewModel : BaseViewModel
    {
        public List<Record> Records { get; set; } = new List<Record>();

        public Record HotRecord { get; set; } = new Record();

        public NewsViewModel()
        {
            LoadRecords();
        }

        private async void LoadRecords()
        {
            var records = await GetRecordsAsync();
            var articleRecords = await GetArticleRecordsAsync();
            var youtubeRecords = await GetYoutubeRecordsAsync();
            var hotRecord = await GetHotRecordAsync();

            Records = records
                .Union(articleRecords)
                .Union(youtubeRecords)
                .ToList();

            HotRecord = hotRecord;
        }

        private async Task<List<Record>> GetRecordsAsync()
        {
            string url = BaseUrl + ApiRoutes.GetRecords;

            var result = await ApiCaller.Get(url);

            if (!string.IsNullOrWhiteSpace(result))
            {
                var records = JsonConvert.DeserializeObject<List<Record>>(result);

                records.ForEach((record) =>
                {
                    record.Image = $"{BaseUrl}/RecordImages/{record.Image}";
                });

                return records;
            }
            else 
            {
                return null;
            }
        }

        private async Task<Record> GetHotRecordAsync()
        {
            string url = BaseUrl + ApiRoutes.GetHotRecord;

            var result = await ApiCaller.Get(url);

            if (!string.IsNullOrWhiteSpace(result))
            {
                var record = JsonConvert.DeserializeObject<Record>(result);

                record.Image = $"{BaseUrl}/RecordImages/{record.Image}";

                return record;
            }
            else 
            {
                return null;
            }
        }

        private async Task<List<Record>> GetArticleRecordsAsync()
        {
            string url = BaseUrl + ApiRoutes.GetArticleRecords;

            var result = await ApiCaller.Get(url);

            if (!string.IsNullOrWhiteSpace(result))
            {
                var records = JsonConvert.DeserializeObject<List<Record>>(result);

                records.ForEach((record) =>
                {
                    record.Image = $"{BaseUrl}/RecordImages/{record.Image}";
                });

                return records;
            }
            else
            {
                return null;
            }
        }

        private async Task<List<Record>> GetYoutubeRecordsAsync()
        {
            string url = BaseUrl + ApiRoutes.GetYoutubeRecords;

            var result = await ApiCaller.Get(url);

            if (!string.IsNullOrWhiteSpace(result))
            {
                var records = JsonConvert.DeserializeObject<List<Record>>(result);

                records.ForEach((record) =>
                {
                    record.Image = $"{BaseUrl}/RecordImages/{record.Image}";
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
