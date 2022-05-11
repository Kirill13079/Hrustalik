using HealthApp.Common.Model;
using HealthApp.Common.Model.Helper;
using HealthApp.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HealthApp.ViewModels
{
    public class NewsViewModel : BaseViewModel
    {
        public List<Record> Records { get; set; }

        public Record HotRecord { get; set; }

        public NewsViewModel()
        {
            LoadRecords();
        }

        private async void LoadRecords()
        {
            string url = BaseUrl + ApiRoutes.GetRecords;
            
            var result = await ApiCaller.Get(url);

            if (!string.IsNullOrWhiteSpace(result))
            {
                var records = JsonConvert.DeserializeObject<List<Record>>(result);

                records.ForEach((record) => record.Image = $"{BaseUrl}/RecordImages/{record.Image}");

                Records = records
                    .Where(x => !x.IsHot)
                    .ToList();

                HotRecord = records
                    .Where(x => x.IsHot)
                    .FirstOrDefault();
            }
        }
    }
}
