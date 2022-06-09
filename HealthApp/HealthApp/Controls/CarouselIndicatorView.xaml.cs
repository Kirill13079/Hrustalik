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
            typeof(CarouselIndicatorView), null);

        public static readonly BindableProperty CurrentItemProperty = BindableProperty.Create(
            nameof(CurrentItem), 
            typeof(object), 
            typeof(CarouselIndicatorView),
            null,
            BindingMode.TwoWay, 
            propertyChanged: CurrentItemChange);

        public IEnumerable Items
        {
            get { return (IEnumerable)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public object CurrentItem
        {
            get { return GetValue(CurrentItemProperty); }
            set { SetValue(CurrentItemProperty, value); }
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
                    x.MoveActiveIndicator(label);
                    return;
                }
            }
        }

        private void MoveActiveIndicator(Label target)
        {
            var width = target.Width - activeIndicator.Width;
            activeIndicator.TranslateTo(target.X + (width / 2), 0, 100, Easing.Linear);

            if (Parent.Parent.Parent is Views.CategoryNewsPage)
            { 
                (Parent.Parent.Parent as Views.CategoryNewsPage).ScrollListCommand.Execute(null);
            }
        }

        private void ChangeTab(object sender, System.EventArgs e)
        {
            foreach (var item in Items)
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