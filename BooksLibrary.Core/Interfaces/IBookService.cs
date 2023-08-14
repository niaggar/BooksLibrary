using BooksLibrary.Model.Enums;
using BooksLibrary.Model.TO;

namespace BooksLibrary.Core.Interfaces
{
    public interface IBookService
    {
        ValueTask<BookTO> GetBook(int id);
        ValueTask<PaginationResultTO<BookTO>> GetBooks(FilterTO? filter, PaginationTO? pagination);
        ValueTask<IEnumerable<BookTO>> GetBooksByUser(int userId, RelationUserBookEnum? relation);
    }
}
