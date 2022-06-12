using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HealthApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AuthorsAndCategoriesPage : ContentPage
    {
        public ICommand ScrollListCommand { get; set; }

        public AuthorsAndCategoriesPage()
        {
            InitializeComponent();

            Shell.SetNavBarIsVisible(this, false);
            Shell.SetTabBarIsVisible(this, false);

            ScrollListCommand = new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    var selectedIndex = vm.TabAuthorsAndCategoriesItems.IndexOf(vm.CurrentTab);

                    await scrollView.ScrollToAsync(60 * selectedIndex, scrollView.ContentSize.Width - scrollView.Width, true);
                });
            });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}