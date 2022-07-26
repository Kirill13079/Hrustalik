using System.Threading.Tasks;
using HealthApp.Helpers;
using HealthApp.Interfaces;
using HealthApp.Views.Dialogs;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;

namespace HealthApp.Service
{
    public class AlertDialogService : IAlertDialog
    {
        private static TaskCompletionSource<bool> _taskCompletionSource;

        public async Task ShowDialogAsync(string title, string message, string accept)
        {
            if (DialogsHelper.ProgressDialog.IsShowing)
            {
                DialogsHelper.ProgressDialog.Hide();
            }

            var dialog = new DisplayAlert(title, message, accept);

            await Application.Current.MainPage.Navigation.PushPopupAsync(dialog);
        }

        public async Task<bool> ShowDialogConfirmationAsync(string title, string message, string accept, string cancel)
        {
            _taskCompletionSource = new TaskCompletionSource<bool>();

            var dialogSheet = new DisplayAlertSheet(title, message, accept, cancel, Callback);

            await Application.Current.MainPage.Navigation.PushPopupAsync(dialogSheet);

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
