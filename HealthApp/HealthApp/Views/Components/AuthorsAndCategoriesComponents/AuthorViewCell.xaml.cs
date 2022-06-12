using HealthApp.Common.Model;
using HealthApp.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HealthApp.Views.Components.AuthorsAndCategoriesComponents
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AuthorViewCell : Grid
    {
        private AuthorsAndCategoriesModel _bindingContext;

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
        }

        private void AuthorCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            var checkBox = (CheckBox)sender;
        }
    }
}