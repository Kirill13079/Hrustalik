using HealthApp.Common.Model;
using HealthApp.Common.Model.Helper;
using HealthApp.Extensions;
using HealthApp.Service;
using HealthApp.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HealthApp.Views.Components.CategoryNewsComponents
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WideNewsViewCell : ViewCell
    {
        private Record _bindingContext = null;

        public WideNewsViewCell()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            image.Source = null;

            _bindingContext = BindingContext as Common.Model.Record;

            image.Source = _bindingContext.Image;
            description.Text = _bindingContext.Name;
            data.Text = _bindingContext.DateAdded.UtcDateTime.ToRelativeDateString(true);
            authorImage.Source = _bindingContext.Author.Logo;
            published.Text = _bindingContext.Author.Name;
            addBookmark.IsVisible = !_bindingContext.IsBookmark;
            deleteBookmark.IsVisible = _bindingContext.IsBookmark;
        }

        private void RecordTapped(object sender, EventArgs e)
        {
            var record = (sender as Grid).BindingContext as Common.Model.Record;

            if (record != null)
            {
                Navigation.NavigateTo("news", record);
            }
        }

        private async void AddBookmarkRecordTapped(object sender, EventArgs e)
        {
            var record = (sender as Frame).BindingContext as Record;
            string url = ApiRoutes.BaseUrl + ApiRoutes.AddBookmark;

            if (record != null)
            {
                var bookmark = new Bookmark { Record = record };
                var response = await ApiCaller.Post(url, bookmark);

                if (!string.IsNullOrWhiteSpace(response))
                {
                    BookmarksNewsViewModel.Instance.BookmarkModel.Records.Add(record);

                    _bindingContext.IsBookmark = true;

                    addBookmark.IsVisible = !_bindingContext.IsBookmark;
                    deleteBookmark.IsVisible = _bindingContext.IsBookmark;
                }
            }
        }

        private async void DeleteBookmarkRecordTapped(object sender, EventArgs e)
        {
            var record = (sender as Frame).BindingContext as Record;
            string url = ApiRoutes.BaseUrl + ApiRoutes.DeleteBookmark + $"/?id={record.Id}";

            if (record != null)
            {
                var response = await ApiCaller.Post(url, "");

                if (!string.IsNullOrWhiteSpace(response))
                {
                    BookmarksNewsViewModel.Instance.BookmarkModel.Records.Remove(record);

                    _bindingContext.IsBookmark = false;

                    addBookmark.IsVisible = !_bindingContext.IsBookmark;
                    deleteBookmark.IsVisible = _bindingContext.IsBookmark;
                }
            }
        }
    }
}