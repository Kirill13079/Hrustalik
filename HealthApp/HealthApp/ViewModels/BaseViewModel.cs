﻿using HealthApp.Common.Model;
using HealthApp.Common.Model.Helper;
using HealthApp.Service;
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

        public ICommand GoBackPageCommand => new Command(async () => 
        {
            await Shell.Current.Navigation.PopAsync();
        });

        public ICommand AddBookmarkCommand => new Command(async (obj) => 
        {
            string url = BaseUrl + ApiRoutes.AddBookmark;

            var bookmark = new Bookmark 
            { 
                Record = obj as Record
            };

            var result = await ApiCaller.Post(url, bookmark);

            if (string.IsNullOrWhiteSpace(result))
            {
                await AppInstance.MainPage.DisplayAlert("test", "test", "ok");
            }

        });

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

        public async void NavigateTo<T, U>(string route, T modelParametr, U selectParameter)
        {
            var modelParameter = string.Empty;
            var selectParametr = string.Empty;

            if (modelParametr != null)
            {
                modelParameter = JsonConvert.SerializeObject(modelParametr);
                modelParameter = Uri.EscapeDataString(modelParameter);
            }

            if (selectParameter != null)
            {
                selectParametr = JsonConvert.SerializeObject(selectParameter);
                selectParametr = Uri.EscapeDataString(selectParametr);
            }

            var state = Shell.Current.CurrentState;

            await Shell.Current.GoToAsync($"{state.Location}/{route}?parameter={modelParameter}&select={selectParametr}");

            Shell.Current.FlyoutIsPresented = false;
        }

        public async void NavigateTo(string route, string title = null)
        {
            var state = Shell.Current.CurrentState;

            await Shell.Current.GoToAsync($"{state.Location}/{route}?title={title}");

            Shell.Current.FlyoutIsPresented = false;
        }
    }
}
