using System;
using System.Linq;
using System.Threading.Tasks;
using HealthApp.Common.Model;
using HealthApp.Common.Model.Helper;
using HealthApp.Helpers;
using HealthApp.Service;
using HealthApp.ViewModels.Data;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HealthApp.Views.Components.PopupComponents
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewsPopup
    {
        private const uint AnimationSpeed = 100;
        private RecordViewModel _currentRecord = null;

        public NewsPopup(RecordViewModel currentRecord)
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

                    App.ViewModelLocator.MainVm.LikeRecordCommand.Execute(_currentRecord);
                    App.ViewModelLocator.CategoryVm.LikeRecordCommand.Execute(_currentRecord);
                    App.ViewModelLocator.BookmarkVm.LikeRecordCommand.Execute(_currentRecord);
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

                    App.ViewModelLocator.MainVm.LikeRecordCommand.Execute(_currentRecord);
                    App.ViewModelLocator.CategoryVm.LikeRecordCommand.Execute(_currentRecord);
                    App.ViewModelLocator.BookmarkVm.LikeRecordCommand.Execute(_currentRecord);
                }
            }

            _currentRecord.IsBookmark = isLiked;

            bookmarkImage.SvgSource = isLiked
                ? "HealthApp.Resources.Icons.likeFull.svg"
                : "HealthApp.Resources.Icons.like.svg";
            bookmarkLablel.Text = isLiked
                ? "Убрать из закладок"
                : "В закладки";

            _ = await bookmarkImage.ScaleTo(scale: 1.2, length: AnimationSpeed);
            _ = await bookmarkImage.ScaleTo(scale: 1, length: AnimationSpeed);
        }

        private async void OpenLinkRecordTapped(object sender, EventArgs e)
        {
            await Service.Navigation.NavigateRemovePopupPageAsync(this);

            DialogsHelper.ProgressDialog.Show();

            await PopupNavigation.Instance.PushAsync(new WebPage(_currentRecord));

            DialogsHelper.ProgressDialog.Hide();
        }
    }
}