using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace BooksLibrary.Web.Utils.Security
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private TokenUtil _tokenUtil;

        public string GetToken()
        {
            return _tokenUtil?.Token;
        }

        public async Task MarkUserAsAuthenticated(TokenUtil token)
        {
            _tokenUtil = token;
            if (!_tokenUtil.IsValid)
            {
                await MarkUserAsLoggedOut();
                return;
            }

            var claims = _tokenUtil.Claims;
            var claimsIdentity = new ClaimsIdentity(claims, "jwt");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));

            await Task.CompletedTask;
        }

        public async Task MarkUserAsLoggedOut()
        {
            _tokenUtil = null;

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal())));

            await Task.CompletedTask;
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            if (_tokenUtil == null || !_tokenUtil.IsValid)
                return Task.FromResult(new AuthenticationState(new ClaimsPrincipal()));

            var claims = _tokenUtil.Claims;
            var claimsIdentity = new ClaimsIdentity(claims, "jwt");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            return Task.FromResult(new AuthenticationState(claimsPrincipal));
        }
    }
}
