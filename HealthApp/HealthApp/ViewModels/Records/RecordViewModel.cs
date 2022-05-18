using HealthApp.Common.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace HealthApp.ViewModels.Records
{
    [QueryProperty("Parameter", "parameter")]
    [QueryProperty("Title", "title")]
    public class RecordViewModel : BaseViewModel
    {
        public Record CurrentRecord { get; set; }

        private string parameter;
        public string Parameter
        {
            get { return parameter; }
            set
            {
                parameter = Uri.UnescapeDataString(value);
                CurrentRecord = JsonConvert.DeserializeObject<Record>(parameter);
            }
        }

        public RecordViewModel()
        { 
        
        }
    }
}
