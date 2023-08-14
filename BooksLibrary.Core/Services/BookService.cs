using BooksLibrary.Core.Interfaces;
using BooksLibrary.Model;
using BooksLibrary.Model.Enums;
using BooksLibrary.Model.Models;
using BooksLibrary.Model.TO;
using Microsoft.EntityFrameworkCore;

namespace BooksLibrary.Core.Services
{
    public class BookService : BaseService<Book>, IBookService
    {
        public BookService(BooksLibraryContext context) : base(context)
        {
        }

        public async ValueTask<BookTO> GetBook(int id)
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

        public async ValueTask<PaginationResultTO<BookTO>> GetBooks(FilterTO? filter, PaginationTO? pagination)
        {
            var query = BuildBaseQuery();

            if (filter != null) ApplyFilter(ref query, filter);

            var totalItems = await query.CountAsync();
            if (pagination != null) ApplyPagination(ref query, pagination);

            var resDB = await query.ToListAsync();
            var totalPages = pagination != null 
                ? CalculateTotalPages(totalItems, pagination.PageSize!.Value) 
                : 1;

            return new PaginationResultTO<BookTO>
            {
                Items = resDB,
                TotalItems = totalItems,
                TotalPages = totalPages,
                Pagination = pagination
            };
        }

        public async ValueTask<IEnumerable<BookTO>> GetBooksByUser(int userId, RelationUserBookEnum? relation)
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
        private IQueryable<BookTO> BuildBaseQuery()
        {
            var query = DbSet
                .Include(x => x.Author)
                .Include(x => x.Genres)
                .OrderBy(x => x.Title)
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

        private void ApplyFilter(ref IQueryable<BookTO> query, FilterTO filter)
        {
            query = filter.Filter switch
            {
                FilterEnum.Author => query.Where(x => x.Author.Name.Contains(filter.Value)),
                FilterEnum.Genre => query.Where(x => x.Genres.Any(g => g.Name.Contains(filter.Value))),
                FilterEnum.Title => query.Where(x => x.Title.Contains(filter.Value)),
                FilterEnum.Year => query.Where(x => x.Year == filter.Value),
                FilterEnum.Pages => query.Where(x => int.Parse(filter.Value) <= x.Pages),
                FilterEnum.None => query
            };
        }

        private void ApplyPagination(ref IQueryable<BookTO> query, PaginationTO pagination)
        {
            var skip = (pagination.Page - 1) * pagination.PageSize;
            query = query.Skip(skip!.Value).Take(pagination.PageSize!.Value);
        }

        private int CalculateTotalPages(int totalItems, int pageSize)
        {
            if (pageSize <= 0)
                return 1;

            return (int)Math.Ceiling((double)totalItems / pageSize);
        }
        #endregion
    }
}
