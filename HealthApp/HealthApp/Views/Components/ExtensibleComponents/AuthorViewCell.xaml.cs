using HealthApp.Common.Model;
using HealthApp.Helpers;
using HealthApp.Models;
using HealthApp.ViewModels;
using HealthApp.ViewModels.Data;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.PancakeView;
using Xamarin.Forms.Xaml;

namespace HealthApp.Views.Components.ExtensibleComponents
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AuthorViewCell : Grid
    {
        private ExtensibleObject _bindingContext;

        public AuthorViewCell()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            _bindingContext = BindingContext as ExtensibleObject;

            if (_bindingContext != null)
            {
                name.Text = _bindingContext.Author.Name;
                authorImage.Source = _bindingContext.Author.Logo;

                SetBorderFrame();
            }
        }

        private async void TapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {
            _ = await AuthorsHelper.GetSavedUserAuthorsAsync();

            if (_bindingContext.IsActive)
            {
                if (AuthorsHelper.SavedUserAuthors.Count == 1)
                {
                    await Application.Current.MainPage.DisplayAlert("Внимание", "Необходимо оставить хотя бы одного автора", "Понятно");
                }
                else
                {
                    _bindingContext.IsActive = false;

                    AuthorsHelper.RemoveUserAuthors(_bindingContext.Author);
                }
            }
            else
            {
                _bindingContext.IsActive = true;

                AuthorsHelper.AddUserAuthors(_bindingContext.Author);
            }

            DialogsHelper.ProgressDialog.Show();

            Task[] tasks =
            {
                App.ViewModelLocator.MainVm.GetDataAsync(),
                App.ViewModelLocator.CategoryVm.GetDataAsync(),
                App.ViewModelLocator.BookmarkVm.GetDataAsync()
            };

            await Task.WhenAll(tasks);

            DialogsHelper.ProgressDialog.Hide();

            SetBorderFrame();
        }

        private void SetBorderFrame()
        {
            if (_bindingContext.IsActive)
            {
                circleFrame.Border = new Border
                {
                    Color = Color.FromHex("#2174F2"),
                    Thickness = 5
                };

                activeChek.IsVisible = true;
            }
            else
            {
                circleFrame.Border = null;
                activeChek.IsVisible = false;
            }
        }
    }
}