using BooksLibrary.Model.TO;
using BooksLibrary.Web.Contracts;
using BooksLibrary.Web.Utils;
using BooksLibrary.Web.Utils.Security;
using BooksLibrary.Web.ViewModel;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using System.Net.Http;

namespace BooksLibrary.Web.Services
{
    public class LoginService : ILoginContract
    {
        private readonly HttpMethodsUtil _httpMethods;
        private readonly AuthenticationStateProvider _authentication;
        private readonly string baseUrl = "Login/";

        public LoginService(HttpMethodsUtil httpMethodsUtil, AuthenticationStateProvider authentication)
        {
            _httpMethods = httpMethodsUtil;
            _authentication = authentication;
        }

        public async Task<UserTO> Authenticate(LoginModel loginModel)
        {
            var loginTo = new LoginTO
            {
                Email = loginModel.Email,
                Password = loginModel.Password
            };

            var resTo = await _httpMethods.PostAsync<ResultTO<UserTO>>($"{baseUrl}Authenticate", loginTo);

            if (resTo != null && resTo.Success)
            {
                var customAuth = _authentication as CustomAuthStateProvider;
                var token = new TokenUtil { Token = resTo.Data.Token };

                await customAuth!.MarkUserAsAuthenticated(token);
            }

            return resTo?.Data;
        }

        public async Task Logout()
        {
            var customAuth = _authentication as CustomAuthStateProvider;
            var token = customAuth!.GetToken();

            await _httpMethods.PostAsync<ResultTO<string>>($"{baseUrl}Logout", jwtToken: token);
            await customAuth!.MarkUserAsLoggedOut();
        }

        public Task<UserTO> Register(UserModel user)
        {
            throw new NotImplementedException();
        }

        public Task<UserTO> RefreshToken(string token)
        {
            throw new NotImplementedException();
        }

        
    }
}
