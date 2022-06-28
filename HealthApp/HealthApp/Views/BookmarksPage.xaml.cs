using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HealthApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BookmarksPage : ContentPage
    {
        public BookmarksPage()
        {
            InitializeComponent();

            BindingContext = App.ViewModelLocator.BookmarkVm;

            var bindingContext = BindingContext as ViewModels.BookmarkViewModel;

            listView.RefreshCommand = bindingContext.RefreshCommand;
        }
    }
}