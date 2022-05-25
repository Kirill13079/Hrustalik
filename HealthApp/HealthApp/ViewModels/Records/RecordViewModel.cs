using HealthApp.Common.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HealthApp.ViewModels.Records
{
    public class RecordViewModel : BaseViewModel
    {
        public Record CurrentRecord { get; set; }

        public ICommand ShareRecordCommand => new Command(async () =>
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Text = CurrentRecord.Source,
                Title = CurrentRecord.Name
            });
        });

        public ICommand OpenLinkCommand => new Command(async (obj) =>
        {
            await Browser.OpenAsync((string)obj);
        });
    }
}
