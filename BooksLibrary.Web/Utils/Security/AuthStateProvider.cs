using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using BooksLibrary.Model.TO;
using BooksLibrary.Web.ViewModel;

namespace BooksLibrary.Web.Utils.Security
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
        private readonly TokenMemoryUtil tokenMemoryUtil;
        public UserModel? CurrentUser = null;

        public AuthStateProvider(TokenMemoryUtil tokenMemory)
        {
            tokenMemoryUtil = tokenMemory;
        }

        public async Task MarkUserAsAuthenticated(UserTO user)
        {
            CurrentUser = new UserModel(user);
            await tokenMemoryUtil.SetCurrentUser(user);

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task MarkUserAsLoggedOut()
        {
            CurrentUser = null;
            await tokenMemoryUtil.RevokeMemoryCredentials();

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            CurrentUser = await tokenMemoryUtil.GetCurrentUserModel();
            if (CurrentUser is null)
            {
                await tokenMemoryUtil.RevokeMemoryCredentials();
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
            else
            {
                var claimsPrincipal = CurrentUser.GetClaimsPrincipal();

                return new AuthenticationState(claimsPrincipal);
            }
        }
    }
}
