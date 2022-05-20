using HealthApp.Common.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace HealthApp.ViewModels.Authors
{
    [QueryProperty("Parameter", "parameter")]
    [QueryProperty("Title", "title")]
    public class AuthorInfoViewModel
    {
        public Author CurrentAuthor { get; set; }

        private string parameter;
        public string Parameter
        {
            get { return parameter; }
            set
            {
                parameter = Uri.UnescapeDataString(value);
                CurrentAuthor = JsonConvert.DeserializeObject<Author>(parameter);
            }
        }
    }
}
