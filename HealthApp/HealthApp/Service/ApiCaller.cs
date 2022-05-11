using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

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

                var request = await client.PostAsync(url, content);

                if (request.IsSuccessStatusCode)
                {
                    return await request.Content.ReadAsStringAsync();
                }
                else
                {
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

                if (request.IsSuccessStatusCode)
                {
                    return await request.Content.ReadAsStringAsync();
                }
                else
                {
                    return null; 
                }
            }
        }
    }
}
