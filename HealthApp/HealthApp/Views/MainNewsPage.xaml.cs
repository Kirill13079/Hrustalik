using HealthApp.Common.Model;
using HealthApp.Models;
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

            BindingContext = ViewModels.MainNewsViewModel.Instance;

            var bindingContext = BindingContext as ViewModels.MainNewsViewModel;

            //var bindingObject = new List<MainTabModel>();

            //bindingObject.Add(bindingContext.MainTabModel);

            //mainScrollView.ItemsSource = bindingObject;

            //BindableLayout.SetItemsSource(test, bindingContext.MainTabModel.Records);
        }
    }
}