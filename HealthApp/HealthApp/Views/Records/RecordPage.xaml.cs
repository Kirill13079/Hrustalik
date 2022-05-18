
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HealthApp.Views.Records
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecordPage : ContentPage
    {
        public RecordPage()
        {
       
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            string xaml = "<StackLayout><Label Text=\"Xamarin Forms\" FontSize=\"24\" Background=\"Red\"/></StackLayout>";
            BodyRecord.LoadFromXaml(xaml);
        }
    }
}