using System.Runtime.CompilerServices;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HealthApp.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CircleCheckboxView : Grid
    {
        public static readonly BindableProperty IsActiveProperty = BindableProperty.Create(
            nameof(IsActive),
            typeof(bool),
            typeof(CarouselIndicatorView),
            default(bool),
            BindingMode.OneWay);

        public object IsActive
        {
            get => GetValue(IsActiveProperty); 
            set => SetValue(IsActiveProperty, value);
        }

        public CircleCheckboxView()
        {
            InitializeComponent();

            checkboxChecked.IsVisible = (bool)IsActive;
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == IsActiveProperty.PropertyName)
            {
                checkboxChecked.IsVisible = (bool)IsActive;
            }
        }
    }
}