using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksLibrary.Model.TO
{
    public class UserTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Picture { get; set; } = "https://avatars.githubusercontent.com/u/58381139?v=4";
        public string Email { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
    }
}
