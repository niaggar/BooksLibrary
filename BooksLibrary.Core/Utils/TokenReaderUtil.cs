using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BooksLibrary.Core.Utils
{
    public class TokenReaderUtil
    {
        public static IEnumerable<Claim> GetTokenClaims(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = jsonToken as JwtSecurityToken;

            return tokenS?.Claims;
        }

        public static (string UserId, string Username, string ExpireAt) GetTokenValues(string token)
        {
            try
            {
                var claims = GetTokenClaims(token);

                var userId = claims.FirstOrDefault(claim => claim.Type == "userId")?.Value;
                var username = claims.FirstOrDefault(claim => claim.Type == "sub")?.Value;
                var expireAt = claims.FirstOrDefault(claim => claim.Type == "exp")?.Value;

                return (userId, username, expireAt);
            }
            catch (Exception)
            {
                return (null, null, null);
            }
        }

        public static bool IsTokenExpired(string token)
        {
            var expireAt = GetTokenValues(token).ExpireAt;

            if (expireAt == null)
            {
                return true;
            }

            var expireAtDateTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expireAt));

            return expireAtDateTime < DateTime.UtcNow;
        }
    }
}
