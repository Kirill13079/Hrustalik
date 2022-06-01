﻿using Xamarin.Forms;

namespace HealthApp.Views.Components.NewsComponents
{
    public class NewsComponentsDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate NewsView { get; set; }
        public DataTemplate WideNewsView { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (((Common.Model.Record)item).IsHot)
                return WideNewsView;
            return NewsView;
        }
    }
}