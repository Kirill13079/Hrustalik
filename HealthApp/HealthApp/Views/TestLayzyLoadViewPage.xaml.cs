using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HealthApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TestLayzyLoadViewPage : ContentPage
    {
        public TestLayzyLoadViewPage()
        {
            InitializeComponent();
            Build();
        }

        void Build()
        {
            for (var i = 0; i < 117; i++)
            {
                var box = new BoxView
                {
                    BackgroundColor = i % 2 == 0 ? Color.White : Color.Black
                };

                uniformGrid.Children.Add(box);
            }
        }
    }
}