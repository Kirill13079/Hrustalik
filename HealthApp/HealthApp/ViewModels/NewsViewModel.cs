using HealthApp.Models;
using MvvmHelpers;
using Newtonsoft.Json;
using System;
using Xamarin.Forms;

namespace HealthApp.ViewModels
{
    [QueryProperty("Parameter", "parameter")]
    public class NewsViewModel : BaseViewModel
    {
        public RecordModel SelectedRecord { get; set; }

        private string parameter;
        public string Parameter
        {
            get { return parameter; }
            set
            {
                parameter = Uri.UnescapeDataString(value);
                SelectedRecord = JsonConvert.DeserializeObject<RecordModel>(parameter);
            }
        }

        public NewsViewModel()
        { 
        
        }
    }
}
