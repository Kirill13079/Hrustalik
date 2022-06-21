using Acr.UserDialogs;
using System;
using System.Threading;
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

        static CancellationTokenSource cts = null;

        public static void CancelActionSheet()
        {
            if (cts?.IsCancellationRequested ?? true)
            { 
                return; 
            }

            cts.Cancel();
        }

        static readonly string UndefinedError = "Что-то пошло не так. Пожалуйста, повторите попытку позже.";
        static readonly string NetworkError = "Сетевая ошибка.";

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
            }

            UserDialogs.Instance.Toast(new ToastConfig(message)
                .SetBackgroundColor(Color.FromHex("#333333"))
                .SetMessageTextColor(Color.White)
                .SetDuration(TimeSpan.FromSeconds(3))
                .SetPosition(ToastPosition.Bottom)
            );
        }

        public static IProgressDialog ProgressDialog = UserDialogs.Instance.Progress(new ProgressDialogConfig
        {
            AutoShow = false,
            CancelText = "Cancel",
            IsDeterministic = false,
            MaskType = MaskType.Black,
            Title = null
        });
    }
}
