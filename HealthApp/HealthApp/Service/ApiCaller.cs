using HealthApp.Helpers;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using static HealthApp.Helpers.DialogsHelper;

namespace HealthApp.Service
{
    public class ApiCaller
    {
        public static async Task<string> Post<T>(string url, T model)
        {
            using (var client = new HttpClient())
            {
                var token = Preferences.Get("token", null);

                if (!string.IsNullOrWhiteSpace(token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token); 
                }

                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                try
                {
                    var request = await client.PostAsync(url, content);

                    if (request.IsSuccessStatusCode)
                    {
                        return await request.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        HandleDialogMessage(Errors.UndefinedError);

                        return null;
                    }
                }
                catch
                {
                    HandleDialogMessage(Errors.NetworkError);
                    
                    return null;
                }
            }
        }

        public static async Task<string> Get(string url)
        {
            using (var client = new HttpClient())
            {
                var token = Preferences.Get("token", null);

                if (!string.IsNullOrWhiteSpace(token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token); 
                }

                var request = await client.GetAsync(url);

                return request.IsSuccessStatusCode ? await request.Content.ReadAsStringAsync() : null;
            }
        }

        public static async Task<string> GetTest(string url)
        {
            using (var client = new HttpClient())
            {
                var request = await client.GetAsync(url);

                return request.IsSuccessStatusCode ? await request.Content.ReadAsStringAsync() : null;
            }
        }
    }
}
