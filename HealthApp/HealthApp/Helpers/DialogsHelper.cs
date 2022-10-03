using Acr.UserDialogs;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace HealthApp.Helpers
{
    public static class DialogsHelper
    {
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
