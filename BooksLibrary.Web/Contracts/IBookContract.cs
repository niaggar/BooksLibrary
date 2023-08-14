using BooksLibrary.Model.TO;

namespace BooksLibrary.Web.Contracts
{
    public interface IBookContract
    {
        List<BookTO> GetBooks(FilterTO? filter, PaginationTO? pagination);
    }
}
