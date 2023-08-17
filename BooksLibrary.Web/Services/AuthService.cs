using BooksLibrary.Model.TO;
using BooksLibrary.Web.Contracts;
using BooksLibrary.Web.Utils;
using BooksLibrary.Web.Utils.Security;
using BooksLibrary.Web.ViewModel;
using Microsoft.AspNetCore.Components.Authorization;

namespace BooksLibrary.Web.Services
{
    public class AuthService : IAuthContract
    {
        private readonly HttpMethodsUtil _httpMethods;
        private readonly AuthStateProvider _authentication;
        private readonly TokenMemoryUtil tokenMemoryUtil;
        private readonly string baseUrl = "Authentication/";

        public AuthService(HttpMethodsUtil httpMethodsUtil, AuthStateProvider authentication, TokenMemoryUtil tokenMemory)
        {
            _httpMethods = httpMethodsUtil;
            _authentication = authentication;
            tokenMemoryUtil = tokenMemory;
        }

        public async Task<UserModel> GetCurrentUser()
        {
            var isTokenValid = await tokenMemoryUtil.IsTokenValid();
            if (!isTokenValid)
            {
                await _authentication!.MarkUserAsLoggedOut();

                return null;
            }

            return await tokenMemoryUtil.GetCurrentUserModel();
        }

        public async Task<UserModel> Authenticate(LoginModel loginModel)
        {
            var loginTo = new LoginTO
            {
                Email = loginModel.Email,
                Password = loginModel.Password
            };

            var resTo = await _httpMethods.PostAsync<ResultTO<UserTO>>($"{baseUrl}Authenticate", loginTo);

            if (resTo != null && resTo.Success)
            {
                await _authentication!.MarkUserAsAuthenticated(resTo.Data);

                return new UserModel(resTo.Data) { Token = resTo.Data.Token };
            }

            return null;
        }

        public async Task Logout()
        {
            var token = await tokenMemoryUtil.GetToken();

            await _authentication!.MarkUserAsLoggedOut();
            await _httpMethods.PostAsync<ResultTO<string>>($"{baseUrl}Logout", jwtToken: token);
        }

        public Task<UserModel> Register(UserModel user)
        {
            throw new NotImplementedException();
        }

        public Task<UserModel> RefreshToken(string token)
        {
            throw new NotImplementedException();
        }
    }
}
