using Xamarin.Forms;

namespace HealthApp.Views.Components.PopularNewsComponents
{
    public class PopularNewsComponentsDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate PopularNewsView { get; set; }
        public DataTemplate WideNewsView { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return PopularNewsView;
        }
    }
}
