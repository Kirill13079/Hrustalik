using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HealthApp.Views.Authorization
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AnonymousPage : ContentPage
    {
        public AnonymousPage()
        {
            InitializeComponent();
        }
    }
}