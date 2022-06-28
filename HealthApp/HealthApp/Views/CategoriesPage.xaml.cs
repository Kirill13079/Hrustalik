using HealthApp.ViewModels;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HealthApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CategoriesPage : ContentPage
    {
        public ICommand ScrollListCommand { get; set; }

        public CategoriesPage()
        {
            InitializeComponent();

            Shell.SetNavBarIsVisible(this, false);

            BindingContext = App.ViewModelLocator.CategoryVm;

            ScrollListCommand = new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    var bindingContext = BindingContext as CategoryViewModel;
                    var selectedIndex = bindingContext.CategoriesTab.IndexOf(bindingContext.CurrentCategoryTab);

                    await scrollView.ScrollToAsync(60 * selectedIndex, scrollView.ContentSize.Width - scrollView.Width, true);
                });
            });
        }
    }
}