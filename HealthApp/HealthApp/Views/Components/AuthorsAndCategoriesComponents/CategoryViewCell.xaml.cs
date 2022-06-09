using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HealthApp.Views.Components.AuthorsAndCategoriesComponents
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CategoryViewCell : ViewCell
    {
        public CategoryViewCell()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            var bindingContext = BindingContext as Common.Model.Category;
        }
    }
}