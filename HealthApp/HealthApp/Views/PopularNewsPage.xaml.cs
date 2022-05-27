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
	public partial class PopularNewsPage : ContentPage
	{
		public PopularNewsPage ()
		{
			InitializeComponent ();
		}

		private void Menu_Tapped(object sender, EventArgs e)
		{
			Shell.Current.FlyoutIsPresented = true;
		}
	}
}