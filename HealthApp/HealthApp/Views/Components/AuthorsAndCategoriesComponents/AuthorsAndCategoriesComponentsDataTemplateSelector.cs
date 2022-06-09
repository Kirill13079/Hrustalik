using HealthApp.Models;
using Xamarin.Forms;

namespace HealthApp.Views.Components.AuthorsAndCategoriesComponents
{
    public class AuthorsAndCategoriesComponentsDataTemplateSelector : DataTemplateSelector
    {
        private readonly DataTemplate _categoriesViewCell;
        private readonly DataTemplate _authorViewCell;

        public AuthorsAndCategoriesComponentsDataTemplateSelector()
        {
            _categoriesViewCell = new DataTemplate(typeof(CategoryViewCell));
            _authorViewCell = new DataTemplate(typeof(AuthorViewCell));
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (((AuthorsAndCategoriesModel)item).Category != null)
            {
                return _categoriesViewCell;
            }

            return _authorViewCell;
        }
    }
}
