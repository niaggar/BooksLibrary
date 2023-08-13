using BooksLibrary.Model.Models;
using BooksLibrary.Model.TO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksLibrary.Core.Interfaces
{
    public interface IUserService
    {
        Task<UserTO> AuthenticateUser(string email, string password);
        Task<UserTO> GetUser(int id);
        Task<UserTO> GetUserByUsername(string username);
        Task<UserTO> CreateUser(UserTO user);
    }
}
