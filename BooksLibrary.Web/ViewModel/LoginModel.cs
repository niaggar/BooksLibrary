using System.ComponentModel.DataAnnotations;

namespace BooksLibrary.Web.ViewModel
{
    public class LoginModel
    {
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool IsValid()
        {
            var notNull = !string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Password);
            if (!notNull) return false;

            var validEmail = new EmailAddressAttribute().IsValid(Email);
            var passwordLength = Password.Length >= 5;

            return notNull && validEmail && passwordLength;
        }
    }
}
