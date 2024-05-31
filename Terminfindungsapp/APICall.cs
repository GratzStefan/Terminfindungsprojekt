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
using System.IO;
using System.Windows.Controls;
using System.Windows;

namespace Terminfindungsapp
{
    public static class APICall
    {
        // Erstellt HTTP-Client
        private static HttpClient GetHttpClient(string url)
        {
            // Creates HTTPClient with wanted URL
            var client = new HttpClient { BaseAddress = new Uri(url) };
            
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }

        // GET-Request
        public static async Task<T> GetAsync<T>(string url, string urlParameters)
        {
            try
            {
                using (var client = GetHttpClient(url))
                {
                    // Get response of Request
                    HttpResponseMessage response = await client.GetAsync(urlParameters);
                    // Checks if Everything went good
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        // Returning Response in wanted Object
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

        // POST-Request
        public static async Task<bool> PostAsync<T>(string url, T data)
        {
            try
            {
                using (var client = GetHttpClient(url))
                {
                    // Formats Object into JSON
                    string json = JsonSerializer.Serialize<T>(data);
                    
                    // Sends POST-Request
                    var response = client.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json")).Result;

                    // Return if everything went good
                    return response.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        // PUT-Request
        public static async Task<bool> PutAsync<T>(string url, T data)
        {
            try
            {
                using (var client = GetHttpClient(url))
                {
                    // Formats Object into JSON
                    string json = JsonSerializer.Serialize(data);
                    // Sends PUT-Request
                    var response = client.PutAsync(url, new StringContent(json, Encoding.UTF8, "application/json")).Result;
                    // Return if everything went good (Checks also if one element got changed)
                    return response.IsSuccessStatusCode && Convert.ToInt32(await response.Content.ReadAsStringAsync()) == 1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        // DELETE-Request
        public static async Task<T> DeleteAsync<T>(string url)
        {
            try
            {
                using (var client = GetHttpClient(url))
                {
                    // Sends DELETE-Request
                    var response = client.DeleteAsync(url).Result;

                    // Returns Response in wanted Object
                    return (T)Convert.ChangeType(await response.Content.ReadAsStringAsync(), typeof(T));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return default(T);
            }
        }
    }
}
