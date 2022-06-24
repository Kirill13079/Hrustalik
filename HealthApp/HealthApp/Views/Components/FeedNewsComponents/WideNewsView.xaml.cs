using HealthApp.Common.Model;
using HealthApp.Common.Model.Helper;
using HealthApp.Extensions;
using HealthApp.Helpers;
using HealthApp.Models;
using HealthApp.Service;
using HealthApp.ViewModels.Main;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HealthApp.Views.Components.FeedNewsComponents
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WideNewsView : ContentView
    {
        private const uint AnimationSpeed = 100;
        private RecordModel _bindingContext = null;

        public WideNewsView()
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

        private void BindingContextPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            bookmarkImage.SvgSource = _bindingContext.IsBookmark
                ? "HealthApp.Resources.Icons.likeFull.svg"
                : "HealthApp.Resources.Icons.like.svg";
        }

        private void RecordModelTapped(object sender, EventArgs e)
        {
            Service.Navigation.NavigateTo("news", _bindingContext, "main");
        }

        private async void ShareRecordModelTapped(object sender, EventArgs e)
        {
            await DialogsHelper.ShareText(_bindingContext.Name, _bindingContext.Source);
        }

        private async void AddOrDeleteBookmarkRecordTapped(object sender, EventArgs e)
        {
            bool isLiked = _bindingContext.IsBookmark;
            string url;

            if (_bindingContext.IsBookmark)
            {
                url = ApiRoutes.BaseUrl + ApiRoutes.DeleteBookmark + $"/?id={_bindingContext.Id}";

                var response = await ApiCaller.Post(url, _bindingContext.Id);

                if (!string.IsNullOrWhiteSpace(response))
                {
                    isLiked = false;
                }
            }
            else
            {
                url = ApiRoutes.BaseUrl + ApiRoutes.AddBookmark;

                var bookmark = new Bookmark { Record = _bindingContext };
                var response = await ApiCaller.Post(url, bookmark);

                if (!string.IsNullOrWhiteSpace(response))
                {
                    isLiked = true;
                }
            }

            MainViewModel.Instance.SetLikeRecord(_bindingContext, isLiked);

            await bookmarkImage.ScaleTo(1.2, AnimationSpeed);
            await bookmarkImage.ScaleTo(1, AnimationSpeed);
        }
    }
}