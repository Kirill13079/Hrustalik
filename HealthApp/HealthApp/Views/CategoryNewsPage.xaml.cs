using System;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HealthApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CategoryNewsPage : ContentPage
    {
        public ICommand ScrollListCommand { get; set; }

        public CategoryNewsPage()
        {
            InitializeComponent();

            Shell.SetNavBarIsVisible(this, false);

            BindingContext = ViewModels.CategoryNewsViewModel.Instance;

            ScrollListCommand = new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    var bindingContext = BindingContext as ViewModels.CategoryNewsViewModel;
                    var selectedIndex = bindingContext.TabCategoriesRecords.IndexOf(bindingContext.CurrentTab);

                    await scrollView.ScrollToAsync(60 * selectedIndex, scrollView.ContentSize.Width - scrollView.Width, true);
                });
            });
        }
    }
}