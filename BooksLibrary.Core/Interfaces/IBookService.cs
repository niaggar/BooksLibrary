using BooksLibrary.Model.Enums;
using BooksLibrary.Model.TO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksLibrary.Core.Interfaces
{
    public interface IBookService
    {
        Task<BookTO> GetBook(int id);
        Task<PaginationResultTO<BookTO>> GetBooks(PaginationTO? pagination);
        Task<PaginationResultTO<BookTO>> GetBooksByAuthor(int authorId, PaginationTO? pagination);
        Task<PaginationResultTO<BookTO>> GetBooksByGenre(string[] genresNames, PaginationTO? pagination);
        Task<IEnumerable<BookTO>> GetBooksByUser(int userId, RelationUserBookEnum? relation);
    }
}
