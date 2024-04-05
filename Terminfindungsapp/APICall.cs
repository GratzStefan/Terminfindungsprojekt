using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Diagnostics;
using System.Text.Json;
using System.Net.Http.Json;
using System.Windows.Markup;

namespace Terminfindungsapp
{
    public static class APICall
    {
        private static HttpClient GetHttpClient(string url)
        {
            var client = new HttpClient { BaseAddress = new Uri(url) };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

        private static async Task<T> GetAsync<T>(string url, string urlParameters)
        {
            try
            {
                using (var client = GetHttpClient(url))
                {
                    HttpResponseMessage response = await client.GetAsync(urlParameters);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        var result = JsonSerializer.Deserialize<T>(json);
                        return result;
                    }

                    return default(T);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return default(T);
            }
        }

        public static async Task<T> RunAsync<T>(string url, string urlParameters)
        {
            return await GetAsync<T>(url, urlParameters);
        }

        public static async Task<bool> PostAsync<T>(string url, T data)
        {
            try
            {
                using (var client = GetHttpClient(url))
                {
                    string json = JsonSerializer.Serialize<T>(data);
                    
                    var response = client.PostAsync(url.Split('/').Last(), new StringContent(json, Encoding.UTF8, "application/json")).Result;

                    return response.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public static async Task<bool> PutAsync<T>(string url)
        {
            try
            {
                using (var client = GetHttpClient(url))
                {
                    var response = client.PutAsync(url.Split('/').Last(), null).Result;
                    return response.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
