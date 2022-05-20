using HealthApp.Common.Model;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace HealthApp.ViewModels.Categories
{
    [QueryProperty("Parameter", "parameter")]
    [QueryProperty("Title", "title")]
    public class CategoryInfoViewModel
    {
        public Category CurrentCategory { get; set; }

        private string parameter;
        public string Parameter
        {
            get { return parameter; }
            set
            {
                parameter = Uri.UnescapeDataString(value);
                CurrentCategory = CurrentCategory.DeserializeObject<Category>(parameter);
            }
        }
    }
}
