using System.Collections;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HealthApp.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CarouselIndicatorView : Grid
    {
        public static readonly BindableProperty ItemsProperty = BindableProperty.Create(
            nameof(Items), 
            typeof(IEnumerable), 
            typeof(CarouselIndicatorView), 
            null);

        public static readonly BindableProperty CurrentItemProperty = BindableProperty.Create(
            nameof(CurrentItem), 
            typeof(object), 
            typeof(CarouselIndicatorView),
            null,
            BindingMode.TwoWay, 
            propertyChanged: CurrentItemChange);

        public IEnumerable Items
        {
            get => (IEnumerable)GetValue(ItemsProperty);
            set => SetValue(ItemsProperty, value);
        }

        public object CurrentItem
        {
            get => GetValue(CurrentItemProperty);
            set => SetValue(CurrentItemProperty, value);
        }

        public CarouselIndicatorView()
        {
            InitializeComponent();
        }

        private static void CurrentItemChange(object bindable, object oldValue, object newValue)
        {
            var x = (CarouselIndicatorView)bindable;
            var labelContainer = x.FindByName("myList") as FlexLayout;

            foreach (Label label in labelContainer.Children)
            {
                var tabGesture = label.GestureRecognizers[0] as TapGestureRecognizer;

                if (newValue == tabGesture.CommandParameter)
                {
                    x.MoveActiveIndicator(target: label);

                    return;
                }
            }
        }

        private void MoveActiveIndicator(Label target)
        {
            double width = target.Width - activeIndicator.Width;

            _ = activeIndicator.TranslateTo(x: target.X + (width / 2), y: 0, length: 100, easing: Easing.Linear);

            if (Parent.Parent.Parent is Views.CategoriesPage)
            {
                var page = Parent.Parent.Parent as Views.CategoriesPage;

                if (page.ScrollListCommand != null)
                {
                    page.ScrollListCommand.Execute(null);
                }
            }
        }

        private void ChangeTab(object sender, System.EventArgs e)
        {
            foreach (object item in Items)
            {
                if (item == ((TappedEventArgs)e).Parameter)
                {
                    CurrentItem = item;

                    return;
                }
            }
        }
    }
}