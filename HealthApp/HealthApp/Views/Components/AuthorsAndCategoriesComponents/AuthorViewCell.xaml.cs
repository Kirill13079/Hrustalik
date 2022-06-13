using HealthApp.Common.Model;
using HealthApp.Helpers;
using HealthApp.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HealthApp.Views.Components.AuthorsAndCategoriesComponents
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AuthorViewCell : Grid
    {
        private AuthorsAndCategoriesModel _bindingContext;
        private bool _isInitialized = false;

        public AuthorViewCell()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            _bindingContext = BindingContext as AuthorsAndCategoriesModel;

            if (_bindingContext != null)
            {
                name.Text = _bindingContext.Author.Name;
                active.IsChecked = _bindingContext.IsActive;
                authorImage.Source = _bindingContext.Author.Logo;
            }

            _isInitialized = true;
        }

        private void AuthorCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            var checkBox = (CheckBox)sender;

            if (_isInitialized)
            {
                if (_bindingContext.IsActive)
                {
                    _bindingContext.IsActive = false;

                    AuthorsAndCategoriesHelper.RemoveUserAuthors(_bindingContext.Author);
                }
                else
                {
                    _bindingContext.IsActive = true;

                    AuthorsAndCategoriesHelper.AddUserAuthors(_bindingContext.Author);
                }
            }
        }
    }
}