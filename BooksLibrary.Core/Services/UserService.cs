using BooksLibrary.Core.Interfaces;
using BooksLibrary.Model;
using BooksLibrary.Model.Models;
using BooksLibrary.Model.TO;
using Microsoft.EntityFrameworkCore;

namespace BooksLibrary.Core.Services
{
    public class UserService : BaseService<User>, IUserService
    {
        public UserService(BooksLibraryContext context) : base(context) { }

        public async Task<UserTO> CreateUser(UserTO user)
        {
            if (user is null)
                throw new ArgumentNullException(nameof(user));

            ValidateUserFields(user);

            if (await UserExists(user.Email, user.Username))
                throw new Exception("User already exists");

            var newUser = new User()
            {
                Username = user.Username,
                Email = user.Email,
                Password = user.Password
            };

            var userDB = await DbSet.AddAsync(newUser);

            return new UserTO() { Id = userDB.Entity.Id, Username = userDB.Entity.Username, Email = userDB.Entity.Email };
        }

        public async Task<UserTO> AuthenticateUser(string mail, string password)
        {
            var user = await DbSet
                .Where(u => u.Email == mail && u.Password == password)
                .Include(u => u.Token)
                .Select(u => new UserTO()
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email,
                    Token = u.Token.Token
                })
                .FirstOrDefaultAsync();

            return user;
        }

        public async Task<UserTO> GetUser(int id)
        {
            var user = await DbSet
                .Where(u => u.Id == id)
                .Select(u => new UserTO()
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email,
                })
                .FirstOrDefaultAsync();

            return user;
        }

        public async Task<UserTO> GetUserByUsername(string username)
        {
            var user = await DbSet
                .Where(u => u.Username == username)
                .Select(u => new UserTO()
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email,
                })
                .FirstOrDefaultAsync();

            return user;
        }



        #region Private Methods
        private void ValidateUserFields(UserTO user)
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
