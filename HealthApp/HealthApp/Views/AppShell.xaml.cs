using HealthApp.Views.Authors;
using HealthApp.Views.Categories;
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
            _routes.Add("record", typeof(RecordPage));
            _routes.Add("categoryInfo", typeof(CategoryInfoPage));
            _routes.Add("authorInfo", typeof(AuthorInfoPage));

            foreach (var route in _routes)
            {
                Routing.RegisterRoute(route.Key, route.Value);
            }
        }

        //private bool redirected;

        //protected override void OnNavigating(ShellNavigatingEventArgs args)
        //{
        //    if (Device.Android.Equals(Device.RuntimePlatform))
        //    {
        //        if (ShellNavigationSource.ShellSectionChanged.Equals(args.Source) && !redirected)
        //        {
        //            FlyoutIsPresented = true;
        //        }
        //        redirected = true;
        //    }
        //    base.OnNavigating(args);
        //}

        //protected override void OnNavigated(ShellNavigatedEventArgs args)
        //{
        //    if (Device.Android.Equals(Device.RuntimePlatform))
        //    {
        //        FlyoutIsPresented = false;
        //        redirected = false;
        //    }
        //    base.OnNavigated(args);
        //}
    }
}