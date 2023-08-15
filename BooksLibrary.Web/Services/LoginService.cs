using BooksLibrary.Model.TO;
using BooksLibrary.Web.Contracts;
using BooksLibrary.Web.Utils;
using BooksLibrary.Web.ViewModel;
using System.Net.Http.Json;

namespace BooksLibrary.Web.Services
{
    public class LoginService : BaseService, ILoginContract
    {
        public LoginService(HttpClient httpClient, WebAlertsUtil webAlerts)
            : base(httpClient, webAlerts, "Login/")
        {
        }

        public async Task<UserTO> Authenticate(LoginModel loginModel)
        {
            var loginTo = new LoginTO
            {
                Email = loginModel.Email,
                Password = loginModel.Password
            };

            var resTo = await PostAsync<ResultTO<UserTO>>("Authenticate", loginTo);

            return resTo!.Data;
        }

        public Task<UserTO> Register(UserModel user)
        {
            throw new NotImplementedException();
        }

        public Task<UserTO> RefreshToken(string token)
        {
            throw new NotImplementedException();
        }

        public Task<string> RevokeToken(string token)
        {
            throw new NotImplementedException();
        }
    }
}
