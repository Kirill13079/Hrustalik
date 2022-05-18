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

        public ICommand GoBackPageCommand => new Command(GoBackCommandHandler);

        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public async void NavigateTo<T>(string route, T modelParametr, string title = null)
        {
            var parameter = string.Empty;

            if (modelParametr != null)
            { 
                parameter = JsonConvert.SerializeObject(modelParametr);
                parameter = Uri.EscapeDataString(parameter);
            }

            var state = Shell.Current.CurrentState;

            await Shell.Current.GoToAsync($"{state.Location}/{route}?parameter={parameter}&title={title}");

            Shell.Current.FlyoutIsPresented = false;
        }

        public async void NavigateTo(string route, string title = null)
        {
            var state = Shell.Current.CurrentState;

            await Shell.Current.GoToAsync($"{state.Location}/{route}?title={title}");

            Shell.Current.FlyoutIsPresented = false;
        }

        public async void GoBackCommandHandler()
        {
            await Shell.Current.Navigation.PopAsync();
        }
    }
}
