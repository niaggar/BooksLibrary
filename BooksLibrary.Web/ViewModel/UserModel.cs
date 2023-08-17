using BooksLibrary.Model.TO;
using System.Security.Claims;

namespace BooksLibrary.Web.ViewModel
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Picture { get; set; }
        public string Token { get; set; }

        public UserModel() { }

        public UserModel(int id, string name, string email, string picture)
        {
            Id = id;
            Name = name;
            Email = email;
            Picture = picture;
        }

        public UserModel(UserTO userto)
        {
            Id = userto.Id;
            Name = userto.Username;
            Email = userto.Email;
            Picture = userto.Picture;
        }

        public UserModel(ClaimsPrincipal claims)
        {
            Id = int.Parse(claims.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            Name = claims.FindFirst(ClaimTypes.Name)?.Value;
            Email = claims.FindFirst(ClaimTypes.Email)?.Value;
            Picture = claims.FindFirst(nameof(Picture))?.Value;
        }

        public ClaimsPrincipal GetClaimsPrincipal()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, Id.ToString()),
                new Claim(ClaimTypes.Name, Name),
                new Claim(ClaimTypes.Email, Email),
                new Claim(nameof(Picture), Picture)
            };

            var claimsIdentity = new ClaimsIdentity(claims, "jwt");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            return claimsPrincipal;
        }


        public override string ToString()
        {
            return $"Id: {Id}, Name: {Name}, Email: {Email}";
        }
    }
}
