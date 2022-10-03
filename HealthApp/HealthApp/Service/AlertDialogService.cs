using System;
using System.Threading.Tasks;
using HealthApp.Interfaces;
using HealthApp.Views.Dialogs;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;

namespace HealthApp.Service
{
    public class AlertDialogService : IAlertDialog
    {
        private static TaskCompletionSource<bool> _taskCompletionSource;

        [Obsolete]
        public async Task ShowDialogAsync(string title, string message)
        {
            DisplayAlert.Instance.Title = title;
            DisplayAlert.Instance.Message = message;

            await DisplayAlert.Instance.ShowAsync();
        }

        [Obsolete]
        public async Task<bool> ShowDialogConfirmationAsync(string title, string message, string accept, string cancel)
        {
            _taskCompletionSource = new TaskCompletionSource<bool>();

            DisplayAlertSheet.Instance.Title = title;
            DisplayAlertSheet.Instance.Message = message;
            DisplayAlertSheet.Instance.Accept = accept;
            DisplayAlertSheet.Instance.Cancel = cancel;

            await DisplayAlertSheet.Instance.ShowAsync(Callback);

            return await _taskCompletionSource.Task;
        }

        private async Task Callback(bool result)
        {
            await Application.Current.MainPage.Navigation.PopPopupAsync();

            if (_taskCompletionSource != null)
            {
                if (!_taskCompletionSource.Task.IsCanceled &&
                    !_taskCompletionSource.Task.IsCompleted &&
                    !_taskCompletionSource.Task.IsFaulted)
                {
                    _taskCompletionSource.SetResult(result);
                }
            }
        }
    }
}
