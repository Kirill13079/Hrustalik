using HealthApp.Extensions;
using HealthApp.Service;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HealthApp.Views.Components.CategoryNewsComponents
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WideNewsViewCell : ViewCell
    {
        public WideNewsViewCell()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            image.Source = null;

            var bindingContext = BindingContext as Common.Model.Record;

            image.Source = bindingContext.Image;
            description.Text = bindingContext.Name;
            data.Text = bindingContext.DateAdded.UtcDateTime.ToRelativeDateString(true);
            authorImage.Source = bindingContext.Author.Logo;
            published.Text = bindingContext.Author.Name;
        }

        private void TappedRecord(object sender, EventArgs e)
        {
            var obj = (sender as Frame).BindingContext as Common.Model.Record;

            if (obj != null)
            {
                Navigation.NavigateTo("news", obj);
            }
        }
    }
}