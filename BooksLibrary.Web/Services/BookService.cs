using BooksLibrary.Model.TO;
using BooksLibrary.Web.Contracts;
using BooksLibrary.Web.Utils;
using System.Net.Http.Headers;

namespace BooksLibrary.Web.Services
{
    public class BookService : BaseService, IBookContract
    {
        public BookService(HttpClient httpClient, WebAlertsUtil webAlerts) 
            : base(httpClient, webAlerts, "Book/")
        {
        }

        public async ValueTask<PaginationResultTO<BookTO>> GetBooks(FilterTO? filter = null, PaginationTO? pagination = null)
        {
            var token = "";

            //SetToken(token);
            var url = UrlUtil.BuildUrl("GetAll", filter, pagination);
            var res = await GetAsync<ResultTO<PaginationResultTO<BookTO>>>(url);

            return res.Data;
        }
    }
}
