using BooksLibrary.Model.TO;
using BooksLibrary.Web.Contracts;

namespace BooksLibrary.Web.Services
{
    public class BookService : BaseService, IBookContract
    {
        public BookService(HttpClient httpClient) : base(httpClient)
        {
        }

        public List<BookTO> GetBooks(FilterTO? filter, PaginationTO? pagination)
        {
            //var url = "api/books";
            //if (filter != null)
            //{
            //    url += $"?filter={filter}";
            //}
            //if (pagination != null)
            //{
            //    url += $"?pagination={pagination}";
            //}
            //var response = Client.GetAsync(url).Result;

            return new List<BookTO>();
        }
    }
}
