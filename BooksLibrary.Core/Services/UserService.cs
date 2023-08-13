using BooksLibrary.Core.Interfaces;
using BooksLibrary.Model;
using BooksLibrary.Model.Models;
using Microsoft.EntityFrameworkCore;

namespace BooksLibrary.Core.Services
{
    public class UserService : BaseService<User>, IUserService
    {
        public UserService(BooksLibraryContext context) : base(context) { }

        public async Task<User> CreateUser(User user)
        {
            if (user is null)
                throw new ArgumentNullException(nameof(user));

            ValidateUserFields(user);

            if (await UserExists(user.Email, user.Username))
                throw new Exception("User already exists");

            var newUser = await DbSet.AddAsync(user);
            await SaveChanges();

            return newUser.Entity;
        }

        public async Task<User> AuthenticateUser(string mail, string password)
        {
            var user = await DbSet
                .Where(u => u.Email == mail && u.Password == password)
                .Include(u => u.Token)
                .Select(u => new User
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email,
                    Token = u.Token
                })
                .FirstOrDefaultAsync();

            return user;
        }

        public async Task<User> GetUser(int id)
        {
            var user = await DbSet
                .Where(u => u.Id == id)
                .Include(u => u.RelationBooks)
                .Select(u => new User
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email,
                    RelationBooks = u.RelationBooks
                })
                .FirstOrDefaultAsync();

            return user;
        }

        public async Task<User> GetUserByUsername(string username)
        {
            var user = await DbSet
                .Where(u => u.Username == username)
                .Include(u => u.RelationBooks)
                .Select(u => new User
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email,
                    RelationBooks = u.RelationBooks
                })
                .FirstOrDefaultAsync();

            return user;
        }



        #region Private Methods
        private void ValidateUserFields(User user)
        {
            if (string.IsNullOrEmpty(user.Username))
                throw new ArgumentNullException(nameof(user.Username));

            if (string.IsNullOrEmpty(user.Email))
                throw new ArgumentNullException(nameof(user.Email));

            if (string.IsNullOrEmpty(user.Password) || user.Password.Length < 5)
                throw new Exception("Password must be at least 5 characters long");
        }

        private async Task<bool> UserExists(string email, string username)
        {
            return await DbSet.AnyAsync(u => u.Email == email || u.Username == username);
        }
        #endregion
    }
}
