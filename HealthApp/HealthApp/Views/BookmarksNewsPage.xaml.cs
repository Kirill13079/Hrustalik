using HealthApp.Models;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HealthApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BookmarksNewsPage : ContentPage
    {
        public BookmarksNewsPage()
        {
            InitializeComponent();

            BindingContext = ViewModels.BookmarksNewsViewModel.Instance;

            var bindingContext = BindingContext as ViewModels.BookmarksNewsViewModel;

            var bindingObject = new List<BookmarkModel>();

            bindingObject.Add(bindingContext.BookmarkModel);

            mainScrollView.ItemsSource = bindingObject;
        }
    }
}