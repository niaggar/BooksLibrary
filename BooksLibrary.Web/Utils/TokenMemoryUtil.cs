using Blazored.LocalStorage;
using BooksLibrary.Core.Utils;
using BooksLibrary.Model.TO;
using BooksLibrary.Web.ViewModel;

namespace BooksLibrary.Web.Utils
{
    public class TokenMemoryUtil
    {
        private readonly ILocalStorageService _storage;
        private string _token;

        public TokenMemoryUtil(ILocalStorageService storage)
        {
            _storage = storage;
        }

        public async Task<string> GetToken()
        {
            if (_token is not null) return _token;
            _token = await _storage.GetItemAsync<string>("token");

            return _token;
        }

        public async Task SetCurrentUser(UserTO user)
        {
            await _storage.SetItemAsync("token", user.Token);
            await _storage.SetItemAsync("user", user);

            _token = user.Token;
        }

        public async Task RevokeMemoryCredentials()
        {
            await _storage.RemoveItemAsync("token");
            await _storage.RemoveItemAsync("user");
        }

        public async Task<bool> IsTokenValid()
        {
            var token = await GetToken();
            if (token is null) return false;

            var isExpired = TokenReaderUtil.IsTokenExpired(token);
            var user = await _storage.GetItemAsync<UserTO>("user");

            return !isExpired && user is not null;
        }

        public async Task<UserModel> GetCurrentUserModel()
        {
            var isTokenValid = await IsTokenValid();

            if (!isTokenValid)
            {
                await RevokeMemoryCredentials();
                return null;
            }

            var user = await _storage.GetItemAsync<UserTO>("user");

            return new UserModel(user) { Token = await GetToken() };
        }
    }
}
