using Acr.UserDialogs;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HealthApp.Helpers
{
    public static class DialogsHelper
    {
        public enum Errors
        {
            NetworkError,
            UndefinedError
        }

        static readonly CancellationTokenSource cts = null;

        public static void CancelActionSheet()
        {
            if (cts?.IsCancellationRequested ?? true)
            {
                return;
            }

            cts.Cancel();
        }

        private static readonly string UndefinedError = "Что-то пошло не так. Пожалуйста, повторите попытку позже.";
        private static readonly string NetworkError = "Сетевая ошибка.";

        public static void HandleDialogMessage(Errors error, string message = "")
        {
            switch (error)
            {
                case Errors.NetworkError:
                    message = NetworkError;
                    break;
                case Errors.UndefinedError:
                    message = UndefinedError;
                    break;
                default:
                    break;
            }

            _ = UserDialogs.Instance.Toast(new ToastConfig(message)
                .SetBackgroundColor(Color.FromHex("#333333"))
                .SetMessageTextColor(Color.White)
                .SetDuration(TimeSpan.FromSeconds(3))
                .SetPosition(ToastPosition.Bottom)
            );
        }

        public static IProgressDialog ProgressDialog = UserDialogs.Instance.Progress(config: new ProgressDialogConfig
        {
            AutoShow = false,
            CancelText = "Cancel",
            IsDeterministic = false,
            MaskType = MaskType.Black,
            Title = null
        });

        public static async Task ShareText(string text, string uri)
        {
            await Share.RequestAsync(request: new ShareTextRequest
            {
                Uri = uri,
                Title = text
            });
        }
    }
}
