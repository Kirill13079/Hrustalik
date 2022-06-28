using HealthApp.Models;
using HealthApp.ViewModels.Data;
using Xamarin.Forms;

namespace HealthApp.Views.Components.AuthorAndCategoryComponents
{
    public class AuthorAndCategoryComponentsDataTemplateSelector : DataTemplateSelector
    {
        private readonly DataTemplate _categoriesViewCell;
        private readonly DataTemplate _authorViewCell;

        public AuthorAndCategoryComponentsDataTemplateSelector()
        {
            _categoriesViewCell = new DataTemplate(typeof(CategoryViewCell));
            _authorViewCell = new DataTemplate(typeof(AuthorViewCell));
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (((AuthorAndCategoryViewModel)item).Category != null)
            {
                return _categoriesViewCell;
            }

            return _authorViewCell;
        }
    }
}
