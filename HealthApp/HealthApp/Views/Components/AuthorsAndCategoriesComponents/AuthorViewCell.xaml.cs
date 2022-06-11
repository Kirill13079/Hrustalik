using HealthApp.Models;
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
    public partial class AuthorViewCell : Grid
    {
        public AuthorViewCell()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            var bindingContext = BindingContext as AuthorsAndCategoriesModel;

            if (bindingContext != null)
            {
                name.Text = bindingContext.Author.Name;
                //description.Text = bindingContext.Author.Description;
                authorImage.Source = bindingContext.Author.Logo;
            }
        }
    }
}