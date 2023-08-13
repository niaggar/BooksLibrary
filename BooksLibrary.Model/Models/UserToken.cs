using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksLibrary.Model.Models
{
    public class UserToken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTime ExpireAt { get; set; }
        public bool Revoked { get; set; }
        public int UserId { get; set; }

        public UserToken(string token, DateTime expireAt, int userId)
        {
            Token = token;
            ExpireAt = expireAt;
            UserId = userId;
        }


        public virtual User User { get; set; }
    }
}
