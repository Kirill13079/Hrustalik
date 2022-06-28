using HealthApp.Models;
using HealthApp.ViewModels;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HealthApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            BindingContext = App.ViewModelLocator.MainVm;

            var bindingContext = BindingContext as MainViewModel;

            var bindingObject = new List<MainTabModel>();

            bindingObject.Add(bindingContext.MainTab);

            mainScrollView.ItemsSource = bindingObject;
        }
    }
}