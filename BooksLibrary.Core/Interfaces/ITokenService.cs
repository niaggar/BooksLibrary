using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksLibrary.Core.Interfaces
{
    public interface ITokenService
    {
        Task<string> GenerateAccessToken(string username, int userId);
        Task<string> GenerateRefreshToken(string token);
        Task<bool> ValidateToken(string token);
        Task RevokeToken(string token);
        Task<(string UserId, string Username)> GetClaims(string token);
    }
}
