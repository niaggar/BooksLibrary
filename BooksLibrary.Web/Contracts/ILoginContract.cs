using BooksLibrary.Model.TO;
using BooksLibrary.Web.ViewModel;

namespace BooksLibrary.Web.Contracts
{
    public interface ILoginContract
    {
        Task<UserTO> Authenticate(LoginModel login);
        Task<UserTO> Register(UserModel user);
        Task<UserTO> RefreshToken(string token);
        Task<string> RevokeToken(string token);
    }
}
