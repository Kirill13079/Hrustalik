using HealthApp.ViewModels;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HealthApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AuthorsAndCategoriesPage : ContentPage
    {
        private readonly AuthorsAndCategoriesViewModel _bindingContext;

        public ICommand ScrollListCommand { get; set; }

        public AuthorsAndCategoriesPage()
        {
            InitializeComponent();

            Shell.SetNavBarIsVisible(this, false);
            Shell.SetTabBarIsVisible(this, false);

            BindingContext = App.ViewModelLocator.AuthorsAndCategoriesVm;

            _bindingContext = BindingContext as AuthorsAndCategoriesViewModel;

            ScrollListCommand = new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    var selectedIndex = _bindingContext.TabAuthorsAndCategoriesItems.IndexOf(_bindingContext.CurrentTab);

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