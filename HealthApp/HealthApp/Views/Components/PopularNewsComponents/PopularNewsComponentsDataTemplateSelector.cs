using Xamarin.Forms;

namespace HealthApp.Views.Components.PopularNewsComponents
{
    public class PopularNewsComponentsDataTemplateSelector : DataTemplateSelector
    {
        private readonly DataTemplate _popularNewsView;

        public PopularNewsComponentsDataTemplateSelector()
        {
            _popularNewsView = new DataTemplate(typeof(PopularNewsViewCell));
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return _popularNewsView;
        }
    }
}
