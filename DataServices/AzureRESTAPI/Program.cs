using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace AzureRESTAPI
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            DataModel dataModel = new DataModel();
            var bearerToken = await GetBearerToken(dataModel.TenantId, dataModel.ClientId, dataModel.ClientSecret);
            var subscriptionResult = await GetApiResponseAsync("https://management.azure.com/subscriptions?api-version=2016-06-01", bearerToken);
            JToken result = JToken.Parse(subscriptionResult)["value"];
            foreach (var item in result.Children())
            {
                Console.WriteLine("Subscription Id : " + item["subscriptionId"].ToString());
            }
        }

        public static async Task<string> GetBearerToken(string tenantId, string clientId, string clientSecret)
        {
            string token = string.Empty;
            using (var httpClient = new HttpClient())
            {
                var url = new Uri(string.Format("https://login.microsoftonline.com/{0}/oauth2/token", tenantId));
                using (var content = new FormUrlEncodedContent(new[]
                         {
                            new KeyValuePair<string, string>("grant_type", "client_credentials"),
                            new KeyValuePair<string, string>("client_id", clientId),
                            new KeyValuePair<string, string>("client_secret", clientSecret),
                            new KeyValuePair<string, string>("resource", "https://management.core.windows.net/")
                    }))
                {
                    content.Headers.Clear();
                    content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                    HttpResponseMessage response = httpClient.PostAsync(url, content).Result;
                    string resultContent = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    dynamic obj = JsonConvert.DeserializeObject<dynamic>(resultContent);
                    token = obj.access_token;
                }
                return await Task.FromResult(token);
            }
        }

        private static async Task<string> GetApiResponseAsync(string api, string brearerToken)
        {
            var result = await GetAsync(api, string.Empty, brearerToken);
            result = JObject.Parse(result).ToString();
            return result;
        }

        public static async Task<string> GetAsync(string baseUrl, string apiResourse = "", string token = null)
        {
            string result = "You did not get any resonse";
            using (HttpClientHandler handler = new HttpClientHandler())
            {
                handler.SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls11 | SslProtocols.Tls;
                handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
                handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
                using (var client = new HttpClient(handler))
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    HttpResponseMessage response = await client.GetAsync(apiResourse);
                    result = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                }
            }
            return result;
        }        

        public static async Task<string> PostAsync(string baseUrl, string apiResourse = "", string token = null)
        {
            string result = string.Empty;
            try
            {
                HttpClient httpClient = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, baseUrl);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                request.Content = new StringContent(apiResourse, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    string error = await response.Content.ReadAsStringAsync();
                    object formatted = JsonConvert.DeserializeObject(error);
                    result = JsonConvert.SerializeObject(formatted, Formatting.Indented);
                    Console.WriteLine(result);
                }
                result = await response.Content.ReadAsStringAsync();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
            return result;
        }
    }
}
