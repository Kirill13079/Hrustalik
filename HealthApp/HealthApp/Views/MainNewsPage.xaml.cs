using HealthApp.Models;
using HealthApp.ViewModels.Main;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HealthApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainNewsPage : ContentPage
    {
        public MainNewsPage()
        {
            InitializeComponent();

            BindingContext = MainViewModel.Instance;

            var bindingContext = BindingContext as MainViewModel;

            var bindingObject = new List<MainTabModel>();

            bindingObject.Add(bindingContext.MainTabModel);

            mainScrollView.ItemsSource = bindingObject;
        }
    }
}