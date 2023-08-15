using BooksLibrary.Core.Utils;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace BooksLibrary.Web.Utils
{
    public class TokenUtil
    {
        public string Token { get; set; }
        public string Username { get => TokenReaderUtil.GetTokenValues(Token).Username; }
        public string UserId { get => TokenReaderUtil.GetTokenValues(Token).UserId; }
        public string ExpireAt { get => TokenReaderUtil.GetTokenValues(Token).ExpireAt; }
        public IEnumerable<Claim> Claims { get => TokenReaderUtil.GetTokenClaims(Token); }

        public bool IsValid { get => !Token.IsNullOrEmpty() && !TokenReaderUtil.IsTokenExpired(Token); }
    }
}
