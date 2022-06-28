using HealthApp.Models;
using HealthApp.ViewModels.Data;
using Xamarin.Forms;

namespace HealthApp.Views.Components.CategoryNewsComponents
{
    public class CategoryNewsComponentsDataTemplateSelector : DataTemplateSelector
    {
        private readonly DataTemplate _normalNewsView;
        private readonly DataTemplate _wideNewsView;

        public CategoryNewsComponentsDataTemplateSelector()
        {
            _normalNewsView = new DataTemplate(typeof(NormalNewsViewCell));
            _wideNewsView = new DataTemplate(typeof(WideNewsViewCell));
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (!((RecordViewModel)item).IsArticle)
            {
                return _wideNewsView;
            }

            return _normalNewsView;
        }
    }
}
