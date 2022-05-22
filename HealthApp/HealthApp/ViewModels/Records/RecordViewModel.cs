using HealthApp.Common.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace HealthApp.ViewModels.Records
{
    public class RecordViewModel : BaseViewModel
    {
        public Record CurrentRecord { get; set; }
    }
}
