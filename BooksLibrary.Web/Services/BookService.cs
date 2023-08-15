using BooksLibrary.Model.TO;
using BooksLibrary.Web.Contracts;
using BooksLibrary.Web.Utils;
using System.Net.Http.Headers;

namespace BooksLibrary.Web.Services
{
    public class BookService : IBookContract
    {
        private readonly HttpMethodsUtil _httpMethods;
        private readonly string baseUrl = "Book/";

        public BookService(HttpMethodsUtil httpMethodsUtil)
        {
            _httpMethods = httpMethodsUtil;
        }

        public async Task<PaginationResultTO<BookTO>> GetBooks(FilterTO? filter = null, PaginationTO? pagination = null)
        {
            var url = UrlUtil.BuildUrl($"{baseUrl}GetAll", filter, pagination);
            var res = await _httpMethods.GetAsync<ResultTO<PaginationResultTO<BookTO>>>(url);

            return res.Data;
        }
    }
}
