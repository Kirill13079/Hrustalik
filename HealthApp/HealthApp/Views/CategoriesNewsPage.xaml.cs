using HealthApp.ViewModels;
using System;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HealthApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CategoriesNewsPage : ContentPage
    {
        public ICommand ScrollListCommand { get; set; }

        public CategoriesNewsPage()
        {
            InitializeComponent();

            Shell.SetNavBarIsVisible(this, false);

            BindingContext = MainViewModel.Instance;

            ScrollListCommand = new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    var bindingContext = BindingContext as MainViewModel;
                    var selectedIndex = bindingContext.TabCategories.IndexOf(bindingContext.CurrentCategoryTab);

                    await scrollView.ScrollToAsync(60 * selectedIndex, scrollView.ContentSize.Width - scrollView.Width, true);
                });
            });
        }
    }
}