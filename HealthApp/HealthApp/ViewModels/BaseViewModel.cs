using HealthApp.Common.Model.Helper;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;

namespace HealthApp.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public string BaseUrl => ApiRoutes.BaseUrl;

        public App AppInstance => (App)Application.Current;

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand GoBackCommand => new Command(GoBack);

        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public async void NavigateTo<T>(string route, T modelParametr, string title = null)
        {
            var parametr = string.Empty;

            if (modelParametr != null)
            { 
                parametr = JsonConvert.SerializeObject(modelParametr);
                parametr = Uri.EscapeDataString(parametr);
            }

            var state = Shell.Current.CurrentState;

            await Shell.Current.GoToAsync($"{state.Location}/{route}?parametr={parametr}&title={title}");

            Shell.Current.FlyoutIsPresented = false;
        }

        public async void NavigateTo(string route, string title = null)
        {
            var state = Shell.Current.CurrentState;

            await Shell.Current.GoToAsync($"{state.Location}/{route}?title={title}");

            Shell.Current.FlyoutIsPresented = false;
        }

        public async void GoBack()
        {
            await Shell.Current.Navigation.PopAsync();
        }
    }
}
