namespace BooksLibrary.Web.Services
{
    public class BaseService
    {
        protected readonly HttpClient Client;

        public BaseService(HttpClient httpClient)
        {
            Client = httpClient;
        }
    }
}
