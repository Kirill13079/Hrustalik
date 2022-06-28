using Xamarin.Forms;

namespace HealthApp.Views.Components.BookmarkNewsComponents
{
    public class BookmarkNewsComponentsDataTemplateSelector : DataTemplateSelector
    {
        private readonly DataTemplate _bookmarkNewsView;

        public BookmarkNewsComponentsDataTemplateSelector()
        {
            _bookmarkNewsView = new DataTemplate(typeof(BookmarkViewCell));
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return _bookmarkNewsView;
        }
    }
}
