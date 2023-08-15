using System.Text.Json.Serialization;

namespace BooksLibrary.Model.TO
{
    public class PaginationTO
    {
        public int? Page { get; set; } = 1;
        public int? PageSize { get; set; } = 20;

        [JsonIgnore]
        public string Query
        {
            get
            {
                if (Page == null || PageSize == null)
                {
                    return "";
                }

                return $"Page={Page}&PageSize={PageSize}";
            }
        }
    }
}
