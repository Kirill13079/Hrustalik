using System;
using HealthApp.Common.Model;
using HealthApp.Common.Model.Helper;
using HealthApp.Extensions;
using HealthApp.Helpers;
using HealthApp.Models;
using HealthApp.Service;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HealthApp.Views.Components.CategoryNewsComponents
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WideNewsViewCell : ViewCell
    {
        private const uint AnimationSpeed = 100;
        private RecordModel _bindingContext = null;

        public WideNewsViewCell()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            image.Source = null;

            _bindingContext = BindingContext as RecordModel;

            image.Source = _bindingContext.Image;
            description.Text = _bindingContext.Name;
            data.Text = _bindingContext.DateAdded.UtcDateTime.ToRelativeDateString(true);
            authorImage.Source = _bindingContext.Author.Logo;
            published.Text = _bindingContext.Author.Name;
            bookmarkImage.SvgSource = _bindingContext.IsBookmark
                ? "HealthApp.Resources.Icons.likeFull.svg"
                : "HealthApp.Resources.Icons.like.svg";

            _bindingContext.PropertyChanged += BindingContextPropertyChanged;
        }

        private void BindingContextPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            bookmarkImage.SvgSource = _bindingContext.IsBookmark 
                ? "HealthApp.Resources.Icons.likeFull.svg" 
                : "HealthApp.Resources.Icons.like.svg";
        }

        private void RecordModelTapped(object sender, EventArgs e)
        {
            var record = (sender as Grid).BindingContext as RecordModel;

            if (record != null)
            {
                Navigation.NavigateTo("news", record);
            }
        }

        private async void ShareRecordModelTapped(object sender, EventArgs e)
        {
            var record = (sender as Frame).BindingContext as RecordModel;

            if (record != null)
            {
                await DialogsHelper.ShareText(record.Name, record.Source);
            }
        }

        private async void AddOrDeleteBookmarkRecordTapped(object sender, EventArgs e)
        {
            var record = (sender as Frame).BindingContext as RecordModel;

            string url;

            if (_bindingContext.IsBookmark)
            {
                url = ApiRoutes.BaseUrl + ApiRoutes.DeleteBookmark + $"/?id={record.Id}";

                if (record != null)
                {
                    var response = await ApiCaller.Post(url, record.Id);

                    if (!string.IsNullOrWhiteSpace(response))
                    {
                        _bindingContext.IsBookmark = false;

                        await bookmarkImage.ScaleTo(1.2, AnimationSpeed);
                        await bookmarkImage.ScaleTo(1, AnimationSpeed);
                    }
                }
            }
            else 
            { 
                url = ApiRoutes.BaseUrl + ApiRoutes.AddBookmark;

                if (record != null)
                {
                    var bookmark = new Bookmark { Record = record };
                    var response = await ApiCaller.Post(url, bookmark);

                    if (!string.IsNullOrWhiteSpace(response))
                    {
                        _bindingContext.IsBookmark = true;

                        await bookmarkImage.ScaleTo(1.2, AnimationSpeed);
                        await bookmarkImage.ScaleTo(1, AnimationSpeed);
                    }
                }
            }
        }
    }
}