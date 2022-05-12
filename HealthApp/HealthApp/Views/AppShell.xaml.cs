using HealthApp.Views.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HealthApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppShell : Shell
    {
        private Dictionary<string, Type> _routes = new Dictionary<string, Type>();

        public AppShell()
        {
            InitializeComponent();
            RegisterRoutes();
        }

        private void RegisterRoutes()
        {
            _routes.Add("AboutRecord", typeof(AboutRecordPage));

            foreach (var route in _routes)
            {
                Routing.RegisterRoute(route.Key, route.Value);
            }
        }
    }
}