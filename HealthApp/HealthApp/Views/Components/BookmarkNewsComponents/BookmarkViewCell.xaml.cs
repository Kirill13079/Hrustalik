using HealthApp.Common.Model;
using HealthApp.Common.Model.Helper;
using HealthApp.Extensions;
using HealthApp.Service;
using HealthApp.ViewModels.Data;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HealthApp.Views.Components.BookmarkNewsComponents
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BookmarkViewCell : ViewCell
    {
        private const uint AnimationSpeed = 100;
        private RecordViewModel _bindingContext = null;

        public BookmarkViewCell()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            _bindingContext = BindingContext as RecordViewModel;

            image.Source = _bindingContext.Image;
            authorImage.Source = _bindingContext.Author.Logo;
            published.Text = _bindingContext.Author.Name;
            description.Text = _bindingContext.Name;
            categoryTitle.Text = $"#{_bindingContext.Category.Name}";
            bookmarkImage.SvgSource = _bindingContext.IsBookmark
                ? "HealthApp.Resources.Icons.likeFull.svg"
                : "HealthApp.Resources.Icons.like.svg";
        }

        private void BindingContextPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            bookmarkImage.SvgSource = _bindingContext.IsBookmark
                ? "HealthApp.Resources.Icons.likeFull.svg"
                : "HealthApp.Resources.Icons.like.svg";
        }

        private void RecordModelTapped(object sender, EventArgs e)
        {
            Navigation.NavigateToAsync("news", _bindingContext, "category");
        }

        private async void AddOrDeleteBookmarkRecordTapped(object sender, EventArgs e)
        {
            string url;

            _ = await bookmarkImage.ScaleTo(1.2, AnimationSpeed);
            _ = await bookmarkImage.ScaleTo(1, AnimationSpeed);

            if (_bindingContext.IsBookmark)
            {
                url = ApiRoutes.BaseUrl + ApiRoutes.DeleteBookmark + $"/?id={_bindingContext.Id}";

                var response = await ApiCaller.Post(url, _bindingContext.Id);

                if (!string.IsNullOrWhiteSpace(response))
                {
                    App.ViewModelLocator.BookmarkVm.LikeRecordCommand.Execute(_bindingContext);
                    App.ViewModelLocator.MainVm.LikeRecordCommand.Execute(_bindingContext);
                    App.ViewModelLocator.CategoryVm.LikeRecordCommand.Execute(_bindingContext);
                }
            }
            else
            {
                url = ApiRoutes.BaseUrl + ApiRoutes.AddBookmark;

                var bookmark = new Bookmark { Record = _bindingContext };
                var response = await ApiCaller.Post(url, bookmark);

                if (!string.IsNullOrWhiteSpace(response))
                {
                    App.ViewModelLocator.BookmarkVm.LikeRecordCommand.Execute(_bindingContext);
                    App.ViewModelLocator.MainVm.LikeRecordCommand.Execute(_bindingContext);
                    App.ViewModelLocator.CategoryVm.LikeRecordCommand.Execute(_bindingContext);
                }
            }
        }
    }
}