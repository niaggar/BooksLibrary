using BooksLibrary.Core.Interfaces;
using BooksLibrary.Model;
using BooksLibrary.Model.Enums;
using BooksLibrary.Model.Models;
using Microsoft.EntityFrameworkCore;

namespace BooksLibrary.Core.Services
{
    public class SeedService : ISeedService
    {
        private readonly BooksLibraryContext _context;

        public SeedService(BooksLibraryContext context)
        {
            _context = context;
        }

        public async Task Seed()
        {
            await Clear();

            var authors = new List<Author>
            {
                new Author { Name = "J.K. Rowling", Description = "British author known for Harry Potter series" },
                new Author { Name = "George Orwell", Description = "English novelist and essayist" },
            };

            var genres = new List<Genre>
            {
                new Genre { Name = "Fantasy", Description = "Magical and mythical elements" },
                new Genre { Name = "Science Fiction", Description = "Imaginative and futuristic concepts" },
                new Genre { Name = "Dystopian", Description = "Undesirable future" },
            };

            var books = new List<Book>
            {
                new Book
                {
                    Title = "Harry Potter and the Sorcerer's Stone",
                    Synopsis = "The first book in the Harry Potter series",
                    CoverUrl = "https://images-na.ssl-images-amazon.com/images/S/compressed.photo.goodreads.com/books/1550337333i/15868.jpg",
                    Isbn = "978-0-7475-3269-6",
                    Pages = 320,
                    Year = "1997",
                    Author = authors[0],
                    Genres = new List<Genre> { genres[0] }
                },
                new Book
                {
                    Title = "1984",
                    Synopsis = "A dystopian novel about totalitarianism and surveillance",
                    Isbn = "978-0-452-28423-4",
                    Pages = 328,
                    Year = "1949",
                    Author = authors[1],
                    Genres = new List<Genre> { genres[1], genres[2] }
                },
            };

            var users = new List<User>
            {
                new User { Username = "user1", Email = "user1@example.com", Password = "12345" },
                new User { Username = "user2", Email = "user2@example.com", Password = "12345" },
            };

            var relationUserBooks = new List<RelationUserBook>
            {
                new RelationUserBook
                {
                    UserId = 1,
                    BookId = 1,
                    Relation = RelationUserBookEnum.Read,
                    Rating = 5,
                    Review = "Absolutely loved this book!",
                    Date = DateTime.Now.AddDays(-30),
                    User = users[0],
                    Book = books[0]
                },
                new RelationUserBook
                {
                    UserId = 2,
                    BookId = 1,
                    Relation = RelationUserBookEnum.ToRead,
                    User = users[1],
                    Book = books[0]
                },
                new RelationUserBook
                {
                    UserId = 1,
                    BookId = 2,
                    Relation = RelationUserBookEnum.Read,
                    Rating = 4,
                    Review = "Great book, but a bit depressing",
                    Date = DateTime.Now.AddDays(-60),
                    User = users[0],
                    Book = books[1]
                },
                new RelationUserBook
                {
                    UserId = 2,
                    BookId = 2,
                    Relation = RelationUserBookEnum.ToRead,
                    User = users[1],
                    Book = books[1]
                },
            };

            await _context.RelationUserBooks.AddRangeAsync(relationUserBooks);
            await _context.SaveChangesAsync();
        }

        public async Task Clear()
        {
            await _context.RelationUserBooks.ExecuteDeleteAsync();
            await _context.Books.ExecuteDeleteAsync();
            await _context.Authors.ExecuteDeleteAsync();
            await _context.Genres.ExecuteDeleteAsync();
            await _context.Users.ExecuteDeleteAsync();

            await _context.SaveChangesAsync();
        }
    }
}
