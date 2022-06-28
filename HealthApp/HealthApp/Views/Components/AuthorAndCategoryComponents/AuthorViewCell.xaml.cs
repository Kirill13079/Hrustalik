using HealthApp.Common.Model;
using HealthApp.Helpers;
using HealthApp.Models;
using HealthApp.ViewModels;
using HealthApp.ViewModels.Data;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HealthApp.Views.Components.AuthorAndCategoryComponents
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AuthorViewCell : Grid
    {
        private AuthorAndCategoryViewModel _bindingContext;

        public AuthorViewCell()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            _bindingContext = BindingContext as AuthorAndCategoryViewModel;

            if (_bindingContext != null)
            {
                name.Text = _bindingContext.Author.Name;
                checkbox.IsActive = _bindingContext.IsActive;
                authorImage.Source = _bindingContext.Author.Logo;
            }
        }

        private async void TapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {
            AuthorsHelper.GetSavedUserAuthors();

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

            await App.ViewModelLocator.MainVm.GetDataAsync();

            DialogsHelper.ProgressDialog.Hide();

            checkbox.IsActive = _bindingContext.IsActive;
        }
    }
}