using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HealthApp.Styles
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Global : ResourceDictionary
    {
        public Global()
        {
            InitializeComponent();
        }
    }
}