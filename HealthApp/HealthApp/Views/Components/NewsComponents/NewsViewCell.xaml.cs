﻿using HealthApp.Extensions;
using Rg.Plugins.Popup.Services;
using System;
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
            data.Text = bindingContext.DateAdded.UtcDateTime.ToRelativeDateString(true);
            authorImage.Source = bindingContext.Author.Logo;
            published.Text = bindingContext.Author.Name;
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var obj = (sender as Label).BindingContext as Common.Model.Record;

            var parentBindingContext = newsViewCell.Parent.Parent.BindingContext;

            if (obj != null)
            {
                await PopupNavigation.Instance.PushAsync(new PopupComponents.NewsPopup());
            }
        }
    }
}