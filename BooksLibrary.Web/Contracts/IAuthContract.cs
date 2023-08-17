using BooksLibrary.Model.TO;
using BooksLibrary.Web.ViewModel;

namespace BooksLibrary.Web.Contracts
{
    public interface IAuthContract
    {
        Task<UserModel> Authenticate(LoginModel login);
        Task<UserModel> Register(UserModel user);
        Task<UserModel> RefreshToken(string token);
        Task Logout();
        Task<UserModel> GetCurrentUser();
    }
}
