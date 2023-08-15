using BooksLibrary.Model.TO;
using BooksLibrary.Web.Utils;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace BooksLibrary.Web.Services
{
    public class BaseService
    {
        protected readonly HttpClient Client;
        protected readonly WebAlertsUtil WebAlerts;
        protected readonly string ControllerUrl;

        public BaseService(HttpClient httpClient, WebAlertsUtil webAlerts)
        {
            Client = httpClient;
            WebAlerts = webAlerts;
        }

        public BaseService(HttpClient httpClient, WebAlertsUtil webAlerts, string controllerUrl)
        {
            Client = httpClient;
            ControllerUrl = controllerUrl;
            WebAlerts = webAlerts;

            Client.BaseAddress = new Uri(Client.BaseAddress, ControllerUrl);
        }

        protected async Task<T> GetAsync<T>(string url) where T : class
        {
            var res = await Client.GetAsync(url);
            if (!res.IsSuccessStatusCode)
            {
                WebAlerts.ShowAlert(res.RequestMessage.ToString());
                return default;
            }

            var resTo = await res.Content.ReadFromJsonAsync<T>();
            return resTo;
        }

        protected async Task<T> PostAsync<T>(string url, object data) where T : class
        {
            var res = await Client.PostAsJsonAsync(url, data);
            if (!res.IsSuccessStatusCode)
            {
                WebAlerts.ShowAlert(res.RequestMessage.ToString());
                return default;
            }

            var resTo = await res.Content.ReadFromJsonAsync<T>();
            return resTo;
        }

        protected void SetToken(string token)
        {
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}
