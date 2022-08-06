using HealthApp.Models;
using HealthApp.ViewModels;
using PanCardView;
using PanCardView.EventArgs;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HealthApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AuthorsAndCategoriesPage : ContentPage
    {
        private readonly AuthorsAndCategoriesViewModel _bindingContext;

        private bool _isSwiped = false;

        public AuthorsAndCategoriesPage()
        {
            InitializeComponent();

            Shell.SetNavBarIsVisible(this, false);
            Shell.SetTabBarIsVisible(this, false);

            BindingContext = App.ViewModelLocator.AuthorsAndCategoriesVm;

            _bindingContext = BindingContext as AuthorsAndCategoriesViewModel;
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

        private void OnPageSizeChanged(object sender, System.EventArgs e)
        {
            SetCurrentActiveTabItem(_bindingContext.CurrentTab);
        }

        private void SetCurrentActiveTabItem(AuthorAndCategoryModel item)
        {
            FlexLayout labelContainer = tabBar.FindByName("itemsTabBar") as FlexLayout;

            foreach (Label label in labelContainer.Children)
            {
                if (_bindingContext.CurrentTab.Title == label.Text)
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

        private void MoveActiveIndicator(Label target)
        {
            double width = target.Width - activeIndicator.Width;

            _ = activeIndicator.TranslateTo(x: target.X + (width / 2), y: 0, length: 100, easing: Easing.Linear);
        }

        private void OnTabItemTapped(object sender, EventArgs e)
        {
            foreach (object item in _bindingContext.TabAuthorsAndCategoriesItems)
            {
                if (item == ((TappedEventArgs)e).Parameter)
                {
                    _bindingContext.CurrentTab = (AuthorAndCategoryModel)item;

                    SetCurrentActiveTabItem(_bindingContext.CurrentTab);

                    return;
                }
            }
        }

        private void carouselView_ItemAppearing(CardsView view, ItemAppearingEventArgs args)
        {
            if (_isSwiped)
            {
                SetCurrentActiveTabItem((AuthorAndCategoryModel)args.Item);

                _isSwiped = !_isSwiped;
            }
        }

        private void carouselView_ItemSwiped(CardsView view, ItemSwipedEventArgs args)
        {
            _isSwiped = true;
        }
    }
}