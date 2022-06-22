using HealthApp.Extensions;
using HealthApp.Models;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HealthApp.Views.Components.FeedNewsComponents
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WideNewsView : ContentView
    {
        public WideNewsView()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            image.Source = null;

            var bindingContext = BindingContext as RecordModel;

            image.Source = bindingContext.Image;
            description.Text = bindingContext.Name;
            data.Text = bindingContext.DateAdded.UtcDateTime.ToRelativeDateString(true);
            authorImage.Source = bindingContext.Author.Logo;
            published.Text = bindingContext.Author.Name;
        }

        private void TappedRecord(object sender, EventArgs e)
        {
            var obj = (sender as Frame).BindingContext as RecordModel;

            if (obj != null)
            {
                Service.Navigation.NavigateTo("news", obj);
            }
        }
    }
}