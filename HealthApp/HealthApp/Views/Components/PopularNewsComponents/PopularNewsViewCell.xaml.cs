﻿using HealthApp.Service;
using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.PancakeView;
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

            image.Source = null;

            var bindingContext = BindingContext as Common.Model.Record;

            image.Source = bindingContext.Image;
            description.Text = bindingContext.Name;
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var obj = (sender as PancakeView).BindingContext as Common.Model.Record;

            var parentBindingContext = popularNewsViewCell.Parent.Parent.BindingContext;

            if (obj != null)
            {
                Navigation.NavigateTo("news", obj);
            }
        }
    }
}