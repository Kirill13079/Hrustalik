using System;
using System.Linq;
using HealthApp.Common.Model;
using HealthApp.Common.Model.Helper;
using HealthApp.Helpers;
using HealthApp.Models;
using HealthApp.Service;
using HealthApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HealthApp.Views.Components.PopupComponents
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CategoryNewsPopup
    {
        private const uint AnimationSpeed = 100;
        private RecordModel _currentRecord = null;

        public CategoryNewsPopup(RecordModel currentRecord)
        {
            InitializeComponent();

            _currentRecord = CategoriesNewsViewModel.Instance.CurrentTab.Records
                .FirstOrDefault(x => x.Equals(currentRecord));

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
            bool @checked = true;
            string url;

            if (_currentRecord.IsBookmark)
            {
                url = ApiRoutes.BaseUrl + ApiRoutes.DeleteBookmark + $"/?id={_currentRecord.Id}";

                if (_currentRecord != null)
                {
                    var response = await ApiCaller.Post(url, _currentRecord.Id);

                    if (!string.IsNullOrWhiteSpace(response))
                    {
                        @checked = false;
                    }
                }
            }
            else 
            {
                url = ApiRoutes.BaseUrl + ApiRoutes.AddBookmark;

                if (_currentRecord != null)
                {
                    var bookmark = new Bookmark { Record = _currentRecord };
                    var response = await ApiCaller.Post(url, bookmark);

                    if (!string.IsNullOrWhiteSpace(response))
                    {
                        @checked = true;
                    }
                }
            }

            _currentRecord.IsBookmark = @checked;

            bookmarkImage.SvgSource = @checked
                ? "HealthApp.Resources.Icons.likeFull.svg"
                : "HealthApp.Resources.Icons.like.svg";
            bookmarkLablel.Text = @checked
                ? "Убрать из закладок"
                : "В закладки";

            await bookmarkImage.ScaleTo(1.2, AnimationSpeed);
            await bookmarkImage.ScaleTo(1, AnimationSpeed);
        }
    }
}