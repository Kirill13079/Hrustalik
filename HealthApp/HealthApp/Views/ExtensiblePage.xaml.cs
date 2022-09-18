using FFImageLoading.Svg.Forms;
using HealthApp.Models;
using HealthApp.ViewModels;
using PanCardView;
using PanCardView.EventArgs;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HealthApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExtensiblePage : ContentPage
    {
        private readonly ExtensibleViewModel _bindingContext;

        private enum ReloadIconState
        {
            Running,
            Default
        }

        public ExtensiblePage()
        {
            InitializeComponent();

            Shell.SetNavBarIsVisible(this, false);
            Shell.SetTabBarIsVisible(this, false);

            BindingContext = App.ViewModelLocator.AuthorsAndCategoriesVm;

            _bindingContext = BindingContext as ExtensibleViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            SizeChanged += OnPageSizeChanged;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            SizeChanged -= OnPageSizeChanged;
        }

        private void OnPageSizeChanged(object sender, EventArgs e)
        {
            SetCurrentActiveTabItem(_bindingContext.CurrentTabExtensibleItem);
        }

        private void SetCurrentActiveTabItem(ExtensibleModel item)
        {
            if (item != null)
            {
                FlexLayout labelContainer = tabBar.FindByName("itemsTabBar") as FlexLayout;

                foreach (Label label in labelContainer.Children)
                {
                    if (_bindingContext.CurrentTabExtensibleItem.Title == label.Text)
                    {
                        TapGestureRecognizer tabGesture = label.GestureRecognizers[0] as TapGestureRecognizer;

                        if (item == tabGesture.CommandParameter)
                        {
                            MoveActiveIndicator(target: label);

                            return;
                        }
                    }
                }
            }
        }

        private void MoveActiveIndicator(Label target)
        {
            double width = target.Width - activeIndicator.Width;

            _ = activeIndicator.TranslateTo(x: target.X + (width / 2), y: 0, length: 100, easing: Easing.Linear);
        }

        private void OnTabItemTapped(object sender, EventArgs e)
        {
            foreach (object item in _bindingContext.TabExtensibleItems)
            {
                if (item == ((TappedEventArgs)e).Parameter)
                {
                    _bindingContext.CurrentTabExtensibleItem = (ExtensibleModel)item;

                    SetCurrentActiveTabItem(_bindingContext.CurrentTabExtensibleItem);

                    return;
                }
            }
        }

        private void OnCarouselViewItemAppearing(CardsView view, ItemAppearingEventArgs args)
        {
            SetCurrentActiveTabItem((ExtensibleModel)args.Item);
        }

        private void OnCarouselViewItemSwiped(CardsView view, ItemSwipedEventArgs args)
        {
        }

        private void SearchEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            //_bindingContext.SearchCommand.Execute(e.NewTextValue);
        }

        private void ReloadTapped(object sender, EventArgs e)
        {
            _bindingContext.ReloadCommand.Execute(null);

            //ReloadImageAnimation(reloadIcon);
        }


        private void ReloadImageAnimation(SvgCachedImage image)
        {
            image.RotateTo(360, 250);
            image.RotateTo(0, 250);
            //image.RotateTo(0, 0);

            //var parentAnimation = new Animation();

            //var rotate = new Animation(v => image.Rotation = v, 0, 360);

            //parentAnimation.Add(0, 1, rotate);

            //parentAnimation.Commit(this, "animate", 2500);
        }
    }
}