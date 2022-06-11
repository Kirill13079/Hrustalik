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
    public partial class AuthorsAndCategoriesPage : ContentPage
    {
        public AuthorsAndCategoriesPage()
        {
            InitializeComponent();

            Shell.SetNavBarIsVisible(this, false);
            Shell.SetTabBarIsVisible(this, false);

            BindingContext = ViewModels.AuthorsAndCategoriesViewModel.Instance;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            //var bindingContext = BindingContext as ViewModels.CategoryNewsViewModel;

            //bindingContext.CurrentTab = bindingContext.TabCategoriesRecords[0];
        }
    }
}