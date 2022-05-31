using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HealthApp.Views.Components.NewsComponents
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewsViewCell : ViewCell
    {
        public NewsViewCell()
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
            source.Text = $"{bindingContext.Source} . ";
            published.Text = bindingContext.Author.Name;
        }

        /// <summary>
        /// THIS IS BAD AND VERY RISKY CODE, BUT THE PERFECT CODE DOES NOT EXIST :)
        /// </summary>
        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            //var obj = (sender as Label).BindingContext as Models.Article;

            //var parentBindingContext = viewCell.Parent.Parent.BindingContext;

            //if (parentBindingContext != null && parentBindingContext == ViewModels.SavedArticlesViewModels.Instance)
            //{
            //    ViewModels.SavedArticlesViewModels.Instance.CurrentArticle = obj;
            //    await PopupNavigation.Instance.PushAsync(new PopupComponents.SavedArticlesSavePopup());
            //}
            //else
            //{
            //    ViewModels.MainFeedViewModel.Instance.CurrentArticle = obj;
            //    await PopupNavigation.Instance.PushAsync(new PopupComponents.MainFeedSavePopup());
            //}
        }
    }
}