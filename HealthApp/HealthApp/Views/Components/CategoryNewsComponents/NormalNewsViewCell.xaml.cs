using HealthApp.Extensions;
using HealthApp.Service;
using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HealthApp.Views.Components.CategoryNewsComponents
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NormalNewsViewCell : ViewCell
    {
        public NormalNewsViewCell()
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

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var obj = (sender as Frame).BindingContext as Common.Model.Record;

            var parentBindingContext = newsViewCell.Parent.Parent.BindingContext;

            if (obj != null)
            {
                await PopupNavigation.Instance.PushAsync(new PopupComponents.NewsPopup());
            }
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