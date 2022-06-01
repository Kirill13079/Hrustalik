using Newtonsoft.Json;
using System;
using Xamarin.Forms;

namespace HealthApp.Service
{
    public class Navigation
    {
        public static async void NavigateTo<T>(string route, T model, string title = null)
        {
            var parameter = string.Empty;

            if (model != null)
            {
                parameter = JsonConvert.SerializeObject(model);
                parameter = Uri.EscapeDataString(parameter);
            }

            ShellNavigationState state = Shell.Current.CurrentState;

            await Shell.Current.GoToAsync($"{state.Location}/{route}?parameter={parameter}&title={title}");

            Shell.Current.FlyoutIsPresented = false;
        }

        public static async void NavigateTo(string route, string title = null)
        {
            ShellNavigationState state = Shell.Current.CurrentState;

            await Shell.Current.GoToAsync($"{state.Location}/{route}?title={title}");

            Shell.Current.FlyoutIsPresented = false;
        }

        public static async void GoBack()
        {
            await Shell.Current.Navigation.PopAsync();
        }

        public static async void GoBackTo<T>(T model)
        {
            var parameter = string.Empty;

            if (model != null)
            {
                parameter = JsonConvert.SerializeObject(model);
                parameter = Uri.EscapeDataString(parameter);
            }

            ShellNavigationState state = Shell.Current.CurrentState;

            await Shell.Current.GoToAsync($"{state.Location}/settings?parameter={parameter}");
        }
    }
}
