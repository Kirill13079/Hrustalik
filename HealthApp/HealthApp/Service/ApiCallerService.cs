using HealthApp.Extensions;
using HealthApp.Interfaces;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace HealthApp.Service
{
    public class ApiCallerService
    {
        private static readonly IAlertDialog _alertDialogService = new AlertDialogService();

        public static async Task<string> Post<T>(string url, T model)
        {
            using (HttpClient client = new HttpClient())
            {
                string token = Preferences.Get("Token", null);

                if (!string.IsNullOrWhiteSpace(token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer", parameter: token);
                }

                StringContent content = new StringContent(
                    content: JsonConvert.SerializeObject(model),
                    encoding: Encoding.UTF8,
                    mediaType: "application/json");

                try
                {
                    HttpResponseMessage request = await client.PostAsync(url, content);

                    if (request.IsSuccessStatusCode)
                    {
                        return await request.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        await _alertDialogService.ShowDialogAsync(title: Resources.Language.Resource.Error, message: request.ToString());

                        return null;
                    }
                }
                catch
                {
                    await _alertDialogService.ShowDialogAsync(title: Resources.Language.Resource.Error, message: Utils.MessageEnum.Error.Network.DisplayName());

                    return null;
                }
            }
        }

        public static async Task<string> Get(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                string token = Preferences.Get("Token", null);

                if (!string.IsNullOrWhiteSpace(token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer", parameter: token);
                }

                HttpResponseMessage request = await client.GetAsync(url);

                return request.IsSuccessStatusCode ? await request.Content.ReadAsStringAsync() : null;
            }
        }

        public static async Task<string> GetTest(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage request = await client.GetAsync(url);

                return request.IsSuccessStatusCode ? await request.Content.ReadAsStringAsync() : null;
            }
        }
    }
}
