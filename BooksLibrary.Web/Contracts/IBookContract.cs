using BooksLibrary.Model.TO;

namespace BooksLibrary.Web.Contracts
{
    public interface IBookContract
    {
        ValueTask<PaginationResultTO<BookTO>> GetBooks(FilterTO? filter = null, PaginationTO? pagination = null);
    }
}
