using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HealthApp.Views.Components.PopularNewsComponents
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopularNewsViewCell : ViewCell
    {
        public PopularNewsViewCell()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            var bindingContext = BindingContext as Common.Model.Record;
        }
    }
}