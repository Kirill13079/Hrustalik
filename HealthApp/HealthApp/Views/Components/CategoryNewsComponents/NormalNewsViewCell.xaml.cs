﻿using HealthApp.Extensions;
using HealthApp.Models;
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

            var bindingContext = BindingContext as RecordModel;

            image.Source = bindingContext.Image;
            description.Text = bindingContext.Name;
            data.Text = bindingContext.DateAdded.UtcDateTime.ToRelativeDateString(true);
            authorImage.Source = bindingContext.Author.Logo;
            published.Text = bindingContext.Author.Name;
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var record = (sender as Frame).BindingContext as RecordModel;

            if (record != null)
            {
                await PopupNavigation.Instance.PushAsync(new PopupComponents.CategoryNewsPopup(record));
            }
        }

        private void TappedRecord(object sender, EventArgs e)
        {
            var record = (sender as Frame).BindingContext as RecordModel;

            if (record != null)
            {
                Navigation.NavigateTo("news", record);
            }
        }
    }
}