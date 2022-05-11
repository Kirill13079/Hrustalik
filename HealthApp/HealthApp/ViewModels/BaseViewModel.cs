using HealthApp.Common.Model.Helper;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace HealthApp.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public string BaseUrl => ApiRoutes.BaseUrl;

        public App AppInstance => (App)Application.Current;

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
