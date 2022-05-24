using HealthApp.Common.Model;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HealthApp.Views.Records
{
    [QueryProperty("Parameter", "parameter")]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecordPage : ContentPage
    {
        public string Parameter
        {
            set
            {
                vm.CurrentRecord = JsonConvert.DeserializeObject<Record>(Uri.UnescapeDataString(value));

                LoadContent();
            }
        }

        public RecordPage()
        {
            InitializeComponent();
        }

        public void LoadContent()
        {
            BodyRecord.Children.Clear();

            ContentView viewLoad = new ContentView().LoadFromXaml(vm.CurrentRecord.TextXAML);

            BodyRecord.Children.Add(viewLoad);
        }
    }
}