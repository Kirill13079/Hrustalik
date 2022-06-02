using Newtonsoft.Json;
using System;
using Xamarin.Forms;

namespace HealthApp.Service
{
    public class Navigation
    {
        /// <summary>
        /// Метод навигации, c параметрами
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="route">Путь к модели</param>
        /// <param name="model">Передаваемая модель в представление</param>
        /// <param name="title">Заголовок представления</param>
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

        /// <summary>
        /// Метод навигации, без параметров
        /// </summary>
        /// <param name="route">Путь к модели</param>
        /// <param name="title">Заголовок представления</param>
        public static async void NavigateTo(string route, string title = null)
        {
            ShellNavigationState state = Shell.Current.CurrentState;

            await Shell.Current.GoToAsync($"{state.Location}/{route}?title={title}");

            Shell.Current.FlyoutIsPresented = false;
        }

        /// <summary>
        /// Метод навигации, вернуться назад
        /// </summary>
        public static async void GoBack()
        {
            await Shell.Current.GoToAsync($"..");
        }
    }
}
