using HealthApp.Models;
using HealthApp.ViewModels;
using PanCardView;
using PanCardView.EventArgs;
using System;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HealthApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CategoriesPage : ContentPage
    {
        private readonly CategoryViewModel _bindingContext;

        public ICommand ScrollListCommand { get; set; }

        public CategoriesPage()
        {
            InitializeComponent();

            Shell.SetNavBarIsVisible(this, false);

            BindingContext = App.ViewModelLocator.CategoryVm;

            _bindingContext = BindingContext as CategoryViewModel;

            ScrollListCommand = new Command(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    int selectedIndex = _bindingContext.CategoriesTab.IndexOf(_bindingContext.CurrentCategoryTab);

                    await scrollView.ScrollToAsync(x: 60 * selectedIndex, y: scrollView.ContentSize.Width - scrollView.Width, animated: true);
                });
            });
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
            SetCurrentActiveTabItem(_bindingContext.CurrentCategoryTab);
        }

        private void SetCurrentActiveTabItem(TabModel item)
        {
            if (item != null)
            {
                FlexLayout labelContainer = tabBar.FindByName("itemsTabBar") as FlexLayout;

                foreach (Label label in labelContainer.Children)
                {
                    if (_bindingContext.CurrentCategoryTab.Title == label.Text)
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

            ScrollListCommand.Execute(null);
        }

        private void OnTabItemTapped(object sender, EventArgs e)
        {
            foreach (object item in _bindingContext.CategoriesTab)
            {
                if (item == ((TappedEventArgs)e).Parameter)
                {
                    _bindingContext.CurrentCategoryTab = (TabModel)item;

                    SetCurrentActiveTabItem(_bindingContext.CurrentCategoryTab);

                    return;
                }
            }
        }

        private void OnCarouselViewItemAppearing(CardsView view, ItemAppearingEventArgs args)
        {
            SetCurrentActiveTabItem((TabModel)args.Item);
        }
    }
}