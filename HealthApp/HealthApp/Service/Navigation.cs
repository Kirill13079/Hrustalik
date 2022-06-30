using Newtonsoft.Json;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HealthApp.Service
{
    public class Navigation
    {
        public static async void NavigateToAsync<T>(string route, T model, string title = null)
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

        public static async Task NavigateToAsync(string route, string title = null)
        {
            ShellNavigationState state = Shell.Current.CurrentState;

            await Shell.Current.GoToAsync($"{state.Location}/{route}?title={title}");

            Shell.Current.FlyoutIsPresented = false;
        }

        public static async Task NavigateRemovePopupPageAsync(PopupPage popupPage)
        {
            if (PopupNavigation.Instance.PopupStack.Any(predicate: page => page.Equals(popupPage)))
            {
                await PopupNavigation.Instance.RemovePageAsync(popupPage, true);
            }
        }

        public static async void GoBackAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
