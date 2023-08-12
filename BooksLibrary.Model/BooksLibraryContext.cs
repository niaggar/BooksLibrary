using BooksLibrary.Model.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BooksLibrary.Model
{
    public class BooksLibraryContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Author> Authors { get; set; }

        public BooksLibraryContext(DbContextOptions<BooksLibraryContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>(x =>
            {
                x.ToTable("Book");

                x.HasKey(p => p.Id);
                x.Property(p => p.Title).IsRequired().HasMaxLength(50);
                x.Property(p => p.Synopsis).IsRequired().HasMaxLength(500);
                x.Property(p => p.CoverUrl).HasMaxLength(500);
                x.Property(p => p.Isbn).HasMaxLength(50);
                x.Property(p => p.Pages).IsRequired();
                x.Property(p => p.Year).HasMaxLength(50).IsRequired();

                x.HasOne(p => p.Author).WithMany(p => p.Books).HasForeignKey(p => p.AuthorId);
                x.HasMany(p => p.Genres).WithMany(p => p.Books);
                x.HasMany(p => p.RelationUsers).WithOne(p => p.Book).HasForeignKey(p => p.BookId);
            });

            modelBuilder.Entity<Genre>(x =>
            {
                x.ToTable("Genre");

                x.HasKey(p => p.Id);
                x.Property(p => p.Name).IsRequired().HasMaxLength(50);
                x.Property(p => p.Description).HasMaxLength(500);

                x.HasMany(p => p.Books).WithMany(p => p.Genres);
            });

            modelBuilder.Entity<Author>(x =>
            {
                x.ToTable("Author");

                x.HasKey(p => p.Id);
                x.Property(p => p.Name).IsRequired().HasMaxLength(50);
                x.Property(p => p.Description).HasMaxLength(500);

                x.HasMany(p => p.Books).WithOne(p => p.Author).HasForeignKey(p => p.AuthorId);
            });

            modelBuilder.Entity<User>(x =>
            {
                x.ToTable("User");

                x.HasKey(p => p.Id);
                x.Property(p => p.Username).IsRequired().HasMaxLength(50);
                x.Property(p => p.Email).IsRequired().HasMaxLength(50);
                x.Property(p => p.Password).IsRequired().HasMaxLength(50);

                x.HasMany(p => p.RelationBooks).WithOne(p => p.User).HasForeignKey(p => p.UserId);
            });

            modelBuilder.Entity<RelationUserBook>(x =>
            {
                x.ToTable("RelationUserBook");

                x.HasKey(p => new { p.UserId, p.BookId });
                x.Property(p => p.UserId).IsRequired();
                x.Property(p => p.BookId).IsRequired();
                x.Property(p => p.Relation).IsRequired();
                x.Property(p => p.Rating);
                x.Property(p => p.Review).HasMaxLength(500);
                x.Property(p => p.Date);

                x.HasOne(p => p.User).WithMany(p => p.RelationBooks).HasForeignKey(p => p.UserId);
                x.HasOne(p => p.Book).WithMany(p => p.RelationUsers).HasForeignKey(p => p.BookId);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
