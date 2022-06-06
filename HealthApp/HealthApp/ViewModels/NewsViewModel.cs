using HealthApp.Common.Model;
using MvvmHelpers;
using Newtonsoft.Json;
using System;
using Xamarin.Forms;

namespace HealthApp.ViewModels
{
    [QueryProperty("Parameter", "parameter")]
    public class NewsViewModel : BaseViewModel
    {
        public Record SelectedRecord { get; set; }

        private string parameter;
        public string Parameter
        {
            get { return parameter; }
            set
            {
                parameter = Uri.UnescapeDataString(value);
                SelectedRecord = JsonConvert.DeserializeObject<Record>(parameter);
            }
        }

        public NewsViewModel()
        { 
        
        }
    }
}
