using HealthApp.Helpers;
using HealthApp.Models;
using HealthApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HealthApp.Views.Components.AuthorAndCategoryComponents
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CategoryViewCell : Grid
    {
        private AuthorsAndCategoriesModel _bindingContext;

        public CategoryViewCell()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            _bindingContext = BindingContext as AuthorsAndCategoriesModel;

            if (_bindingContext != null)
            {
                name.Text = _bindingContext.Category.Name;
                checkbox.IsActive = _bindingContext.IsActive;
                categoryImage.Source = _bindingContext.Category.Image;
            }
        }

        private async void TapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {
            CategoriesHelper.GetSavedUserCategories();

            if (_bindingContext.IsActive)
            {
                if (CategoriesHelper.SavedUserCategories.Count == 1)
                {
                    
                }
                else
                {
                    _bindingContext.IsActive = false;

                    CategoriesHelper.RemoveUserCategory(_bindingContext.Category);
                }
            }
            else
            {
                _bindingContext.IsActive = true;

                CategoriesHelper.AddUserCategory(_bindingContext.Category);
            }

            DialogsHelper.ProgressDialog.Show();

            await MainViewModel.Instance.GetDataAsync();

            DialogsHelper.ProgressDialog.Hide();

            checkbox.IsActive = _bindingContext.IsActive;
        }
    }
}