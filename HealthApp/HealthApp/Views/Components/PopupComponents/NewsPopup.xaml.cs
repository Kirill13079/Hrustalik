using System;
using HealthApp.Common.Model;
using HealthApp.Common.Model.Helper;
using HealthApp.Helpers;
using HealthApp.Models;
using HealthApp.Service;
using HealthApp.ViewModels.Main;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HealthApp.Views.Components.PopupComponents
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewsPopup
    {
        private const uint AnimationSpeed = 100;
        private RecordModel _currentRecord = null;

        public NewsPopup(RecordModel currentRecord)
        {
            InitializeComponent();

            _currentRecord = currentRecord;

            bookmarkImage.SvgSource = _currentRecord.IsBookmark
                ? "HealthApp.Resources.Icons.likeFull.svg"
                : "HealthApp.Resources.Icons.like.svg";
            bookmarkLablel.Text = _currentRecord.IsBookmark
                ? "Убрать из закладок"
                : "В закладки";
        }

        private async void ShareRecordModelTapped(object sender, EventArgs e)
        {
            await DialogsHelper.ShareText(_currentRecord.Name, _currentRecord.Source);
        }

        private async void AddOrDeleteBookmarkRecordTapped(object sender, EventArgs e)
        {
            bool isLiked = _currentRecord.IsBookmark;
            string url;

            if (_currentRecord.IsBookmark)
            {
                url = ApiRoutes.BaseUrl + ApiRoutes.DeleteBookmark + $"/?id={_currentRecord.Id}";

                var response = await ApiCaller.Post(url, _currentRecord.Id);

                if (!string.IsNullOrWhiteSpace(response))
                {
                    isLiked = false;
                }
            }
            else 
            {
                url = ApiRoutes.BaseUrl + ApiRoutes.AddBookmark;

                var bookmark = new Bookmark { Record = _currentRecord };
                var response = await ApiCaller.Post(url, bookmark);

                if (!string.IsNullOrWhiteSpace(response))
                {
                    isLiked = true;
                }
            }

            MainViewModel.Instance.SetLikeRecord(_currentRecord, isLiked);

            _currentRecord.IsBookmark = isLiked;

            bookmarkImage.SvgSource = isLiked
                ? "HealthApp.Resources.Icons.likeFull.svg"
                : "HealthApp.Resources.Icons.like.svg";
            bookmarkLablel.Text = isLiked
                ? "Убрать из закладок"
                : "В закладки";

            await bookmarkImage.ScaleTo(1.2, AnimationSpeed);
            await bookmarkImage.ScaleTo(1, AnimationSpeed);
        }

        private async void OpenLinkRecordTapped(object sender, EventArgs e)
        {
            //await PopupNavigation.Instance.PopAsync();
            await Navigation.PushAsync(new WebNewsPage(_currentRecord));

            //await DialogsHelper.OpenBrowser(_currentRecord.Source);
        }
    }
}