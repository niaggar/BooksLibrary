namespace BooksLibrary.Web.Utils
{
    public class TokenUtil
    {
        public string Token { get; protected set; }

        public void SetToken(string token)
        {
            Token = token;
        }

        public void ClearToken()
        {
            Token = null;
        }


    }
}
