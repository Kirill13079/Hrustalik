using HealthApp.Extensions;
using HealthApp.Helpers;
using HealthApp.Models;
using HealthApp.Service;
using HealthApp.Utils;
using HealthApp.ViewModels;
using HealthApp.ViewModels.Data;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.PancakeView;
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
                categoryImage.Source = _bindingContext.Category.Image;

                SetBorderFrame();
            }
        }

        private async void TapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {
            _ = await CategoriesHelper.GetSavedUserCategoriesAsync();

            if (_bindingContext.IsActive)
            {
                if (CategoriesHelper.SavedUserCategories.Count == 1)
                {
                    //await AlertDialogService.ShowDialogAsync("fdfd","dfdf","dfdfd");
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