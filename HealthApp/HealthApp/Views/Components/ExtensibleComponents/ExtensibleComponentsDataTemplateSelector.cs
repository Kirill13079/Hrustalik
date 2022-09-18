using HealthApp.ViewModels.Data;
using Xamarin.Forms;

namespace HealthApp.Views.Components.ExtensibleComponents
{
    public class ExtensibleComponentsDataTemplateSelector : DataTemplateSelector
    {
        private readonly DataTemplate _categoriesViewCell;
        private readonly DataTemplate _authorViewCell;

        public ExtensibleComponentsDataTemplateSelector()
        {
            _categoriesViewCell = new DataTemplate(typeof(CategoryViewCell));
            _authorViewCell = new DataTemplate(typeof(AuthorViewCell));
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return ((ExtensibleObject)item).Category != null ? _categoriesViewCell : _authorViewCell;
        }
    }
}
