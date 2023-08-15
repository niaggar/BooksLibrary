using BooksLibrary.Model.TO;
using System.Text;

namespace BooksLibrary.Web.Utils
{
    public class UrlUtil
    {
        public static string BuildUrl(string url, params object[] args)
        {
            var sb = new StringBuilder(url);

            if (args.Length > 0)
            {
                sb.Append("?");

                for (int i = 0; i < args.Length; i++)
                {
                    sb.Append(args[i].ToString());

                    if (i < args.Length - 1)
                    {
                        sb.Append("&");
                    }
                }
            }

            return sb.ToString();
        }

        public static string BuildUrl(string url, FilterTO? filter, PaginationTO? pagination)
        {
            var sb = new StringBuilder(url);

            if (filter != null)
            {
                sb.Append(filter.Query);
            }

            if (pagination != null)
            {
                sb.Append("?" + pagination.Query);
            }

            return sb.ToString();
        }
    }
}
