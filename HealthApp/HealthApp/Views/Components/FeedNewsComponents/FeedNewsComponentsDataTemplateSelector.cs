using Xamarin.Forms;

namespace HealthApp.Views.Components.FeedNewsComponents
{
    public class FeedNewsComponentsDataTemplateSelector : DataTemplateSelector
    {
        private readonly DataTemplate _wideNewsView;

        public FeedNewsComponentsDataTemplateSelector()
        {
            _wideNewsView = new DataTemplate(typeof(WideNewsView));
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return _wideNewsView;
        }
    }
}
