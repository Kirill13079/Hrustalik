using HealthApp.Helpers;
using HealthApp.Models;
using HealthApp.ViewModels;
using HealthApp.ViewModels.Data;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HealthApp.Views.Components.AuthorAndCategoryComponents
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CategoryViewCell : Grid
    {
        private AuthorAndCategoryViewModel _bindingContext;

        public CategoryViewCell()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            _bindingContext = BindingContext as AuthorAndCategoryViewModel;

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

            Task[] tasks =
            {
                App.ViewModelLocator.MainVm.GetDataAsync(),
                App.ViewModelLocator.CategoryVm.GetDataAsync(),
                App.ViewModelLocator.BookmarkVm.GetDataAsync()
            };

            await Task.WhenAll(tasks);

            DialogsHelper.ProgressDialog.Hide();

            checkbox.IsActive = _bindingContext.IsActive;
        }
    }
}