using BooksLibrary.Model.Enums;
using System.Text.Json.Serialization;

namespace BooksLibrary.Model.TO
{
    public class FilterTO
    {
        public string? Value { get; set; } = "";
        public FilterEnum? Filter { get; set; } = FilterEnum.None;

        [JsonIgnore]
        public string Query
        {
            get
            {
                if (Filter == FilterEnum.None)
                {
                    return "";
                }

                return $"{(int)Filter}/{Value}";
            }
        }
    }
}
