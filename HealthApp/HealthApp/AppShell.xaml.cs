using HealthApp.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HealthApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppShell : Shell
    {
        Dictionary<string, Type> _routes = new Dictionary<string, Type>();

        public AppShell()
        {
            InitializeComponent();
            RegisterRoutes();
        }

        private void RegisterRoutes()
        {
            _routes.Add("login", typeof(LoginPage));
            _routes.Add("news", typeof(NewsPage));
            _routes.Add("authorsAndCategories", typeof(AuthorsAndCategoriesPage));

            foreach (var item in _routes)
            {
                Routing.RegisterRoute(item.Key, item.Value);
            }
        }
    }
}