using BooksLibrary.Core.Interfaces;
using BooksLibrary.Model;
using BooksLibrary.Model.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BooksLibrary.Core.Services
{
    public class TokenService : BaseService<UserToken>, ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(BooksLibraryContext context, IConfiguration configuration) : base(context)
        {
            _configuration = configuration;
        }

        public async Task<string> GenerateAccessToken(string username, int userId)
        {
            var actualDate = DateTime.Now;
            var timeSpan = TimeSpan.FromHours(1);
            var expireAt = actualDate.Add(timeSpan);

            var token = GenerateToken(username, userId, actualDate, expireAt);
            var existingToken = await DbSet.FirstOrDefaultAsync(ut => ut.UserId == userId);
            if (existingToken != null)
            {
                existingToken.Token = token;
                existingToken.ExpireAt = expireAt;
                existingToken.Revoked = false;
            }
            else
            {
                await DbSet.AddAsync(new UserToken(token, expireAt, userId));
            }

            await SaveChanges();

            return token;
        }

        public async Task<string> GenerateRefreshToken(string token)
        {
            var (userId, username, _) = GetTokenClaims(token);

            var actualDate = DateTime.Now;
            var timeSpan = TimeSpan.FromHours(1);
            var expireAt = actualDate.Add(timeSpan);

            var refreshToken = GenerateToken(username, int.Parse(userId), actualDate, expireAt);

            var existingToken = await DbSet.FirstOrDefaultAsync(ut => ut.UserId == int.Parse(userId));
            if (existingToken != null)
            {
                existingToken.Token = refreshToken;
                existingToken.ExpireAt = expireAt;
                existingToken.Revoked = false;
            }
            else
            {
                return null;
            }

            await SaveChanges();

            return refreshToken;
        }

        public async Task RevokeToken(string token)
        {
            var (userId, username, _) = GetTokenClaims(token);
            var userToken = DbSet.FirstOrDefault(ut => ut.UserId == int.Parse(userId) && ut.Token == token);

            if (userToken != null)
            {
                userToken.Revoked = true;
                await SaveChanges();
            }
        }

        public async Task<bool> ValidateToken(string token)
        {
            var (userId, username, _) = GetTokenClaims(token);
            var userToken = await DbSet.FirstOrDefaultAsync(ut => ut.UserId == int.Parse(userId));

            if (userToken.Token.Equals(token) == false)
            {
                return false;
            }

            if (userToken == null)
            {
                return false;
            }

            if (userToken.Revoked)
            {
                return false;
            }

            if (userToken.ExpireAt < DateTime.Now)
            {
                return false;
            }

            return true;
        }

        public async Task<(string UserId, string Username, string ExpireAt)> GetClaims(string token)
        {
            return GetTokenClaims(token);
        }


        #region Private Methods
        private (string UserId, string Username, string ExpireAt) GetTokenClaims(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = jsonToken as JwtSecurityToken;

            var userId = tokenS.Claims.First(claim => claim.Type == "userId").Value;
            var username = tokenS.Claims.First(claim => claim.Type == "sub").Value;
            var expireAt = tokenS.Claims.First(claim => claim.Type == "exp").Value;

            return (userId, username, expireAt);
        }

        private string GenerateToken(string username, int userId, DateTime actualDate, DateTime expireAt)
        {
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim("userId", userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat,
                    new DateTimeOffset(actualDate).ToUniversalTime().ToUnixTimeSeconds().ToString(),
                    ClaimValueTypes.Integer64
                ),
                new Claim(JwtRegisteredClaimNames.Exp,
                    new DateTimeOffset(expireAt).ToUniversalTime().ToUnixTimeSeconds().ToString(),
                    ClaimValueTypes.Integer64
                )
            };

            var issuer = _configuration["AuthenticationSettings:Issuer"];
            var audience = _configuration["AuthenticationSettings:Audience"];
            var signinKey = _configuration["AuthenticationSettings:SigningKey"];

            var jwt = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                notBefore: actualDate,
                expires: expireAt,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(signinKey)),
                    SecurityAlgorithms.HmacSha256Signature
                )
            );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
        #endregion
    }
}
