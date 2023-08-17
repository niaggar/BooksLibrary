using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace BooksLibrary.Web.Utils
{
    public class HttpMethodsUtil
    {
        private HttpClient Client { get; set; }
        private WebAlertsUtil WebAlerts { get; set; }

        public HttpMethodsUtil(HttpClient client)
        {
            Client = client;
            WebAlerts = new WebAlertsUtil();
        }

        public async Task<T> GetAsync<T>(string url, string? jwtToken = null) where T : class
        {
            if (!string.IsNullOrEmpty(jwtToken))
                Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

            var res = await Client.GetAsync(url);
            if (!res.IsSuccessStatusCode)
            {
                WebAlerts.ShowAlert(res.RequestMessage.ToString());
                return default;
            }

            var resTo = await res.Content.ReadFromJsonAsync<T>();

            if (!string.IsNullOrEmpty(jwtToken))
                Client.DefaultRequestHeaders.Authorization = null;

            return resTo;
        }

        public async Task<T> PostAsync<T>(string url, object? data = null, string? jwtToken = null) where T : class
        {
            if (!string.IsNullOrEmpty(jwtToken))
                Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

            var res = await Client.PostAsJsonAsync(url, data);
            if (!res.IsSuccessStatusCode)
            {
                WebAlerts.ShowAlert(res.RequestMessage.ToString());
                return default;
            }

            var resTo = await res.Content.ReadFromJsonAsync<T>();

            if (!string.IsNullOrEmpty(jwtToken))
                Client.DefaultRequestHeaders.Authorization = null;

            return resTo;
        }
    }
}
