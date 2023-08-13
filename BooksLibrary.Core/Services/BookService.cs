using BooksLibrary.Core.Interfaces;
using BooksLibrary.Model;
using BooksLibrary.Model.Enums;
using BooksLibrary.Model.Models;
using BooksLibrary.Model.TO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksLibrary.Core.Services
{
    public class BookService : BaseService<Book>, IBookService
    {
        public BookService(BooksLibraryContext context) : base(context)
        {
        }

        public async Task<BookTO> GetBook(int id)
        {
            var resDB = await DbSet
                .Include(x => x.Author)
                .Include(x => x.Genres)
                .Include(x => x.RelationUsers)
                .Select(x => new BookTO
                 {
                     Id = x.Id,
                     Title = x.Title,
                     Synopsis = x.Synopsis,
                     CoverUrl = x.CoverUrl,
                     Year = x.Year,
                     Pages = x.Pages,
                     ISBN = x.Isbn,
                     Author = new AuthorTO
                     {
                         Id = x.Author.Id,
                         Name = x.Author.Name,
                         Description = x.Author.Description
                     },
                     Genres = x.Genres.Select(g => new GenreTO { Name = g.Name, Description = g.Description }).ToArray()
                 })
                .FirstOrDefaultAsync(x => x.Id == id);

            return resDB;
        }

        public async Task<PaginationResultTO<BookTO>> GetBooks(PaginationTO? pagination)
        {
            var query = GetQuery();

            if (pagination != null)
                query = query.Skip((pagination.Page.Value - 1) * pagination.PageSize.Value).Take(pagination.PageSize.Value);

            var totalBooks = await query.CountAsync();
            var totalPage = (int)Math.Ceiling((double)totalBooks / pagination.PageSize.Value);
            var resDB = await query.ToListAsync();

            return new PaginationResultTO<BookTO>
            {
                TotalItems = totalBooks,
                TotalPages = totalPage,
                Items = resDB,
                Pagination = pagination
            };
        }

        public async Task<PaginationResultTO<BookTO>> GetBooksByAuthor(int authorId, PaginationTO? pagination)
        {
            var query = GetQuery().Where(x => x.Author.Id == authorId);

            if (pagination != null)
                query = query.Skip((pagination.Page.Value - 1) * pagination.PageSize.Value).Take(pagination.PageSize.Value);

            var totalBooks = await query.CountAsync();
            var totalPage = (int)Math.Ceiling((double)totalBooks / pagination.PageSize.Value);
            var resDB = await query.ToListAsync();
            
            return new PaginationResultTO<BookTO>
            {
                TotalItems = totalBooks,
                TotalPages = totalPage,
                Items = resDB,
                Pagination = pagination
            };
        }

        public async Task<PaginationResultTO<BookTO>> GetBooksByGenre(string[] genresNames, PaginationTO? pagination)
        {
            var query = GetQuery().Where(x => x.Genres.Any(g => genresNames.Contains(g.Name)));

            if (pagination != null)
                query = query.Skip((pagination.Page.Value - 1) * pagination.PageSize.Value).Take(pagination.PageSize.Value);

            var totalBooks = await query.CountAsync();
            var totalPage = (int)Math.Ceiling((double)totalBooks / pagination.PageSize.Value);
            var resDB = await query.ToListAsync();

            return new PaginationResultTO<BookTO>
            {
                TotalItems = totalBooks,
                TotalPages = totalPage,
                Items = resDB,
                Pagination = pagination
            };
        }

        public async Task<IEnumerable<BookTO>> GetBooksByUser(int userId, RelationUserBookEnum? relation)
        {
            var query = DbSet
                .Include(x => x.Author)
                .Include(x => x.Genres)
                .Include(x => x.RelationUsers)
                .Where(x => x.RelationUsers.Any(r => r.UserId == userId));

            if (relation != null)
            {
                query = query.Where(x => x.RelationUsers.Any(r => r.Relation == relation));
            }

            var resDB = await query.Select(x => new BookTO
            {
                Id = x.Id,
                Title = x.Title,
                CoverUrl = x.CoverUrl,
                Year = x.Year,
                Pages = x.Pages,
                ISBN = x.Isbn,
                Author = new AuthorTO
                {
                    Name = x.Author.Name,
                },
                Genres = x.Genres.Select(g => new GenreTO { Name = g.Name }).ToArray()
            }).ToListAsync();

            return resDB;
        }


        #region Private Methods
        private IQueryable<BookTO> GetQuery()
        {
            var query = DbSet
                .Include(x => x.Author)
                .Include(x => x.Genres)
                .Select(x => new BookTO
                {
                    Id = x.Id,
                    Title = x.Title,
                    CoverUrl = x.CoverUrl,
                    Year = x.Year,
                    Pages = x.Pages,
                    ISBN = x.Isbn,
                    Author = new AuthorTO
                    {
                        Name = x.Author.Name,
                    },
                    Genres = x.Genres.Select(g => new GenreTO { Name = g.Name }).ToArray()
                });

            return query;
        }
        #endregion
    }
}
