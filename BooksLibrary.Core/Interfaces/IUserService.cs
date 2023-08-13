using BooksLibrary.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksLibrary.Core.Interfaces
{
    public interface IUserService
    {
        Task<User> AuthenticateUser(string email, string password);
        Task<User> GetUser(int id);
        Task<User> GetUserByUsername(string username);
        Task<User> CreateUser(User user);
    }
}
