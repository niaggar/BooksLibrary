using BooksLibrary.Model.Enums;

namespace BooksLibrary.Model.TO
{
    public class FilterTO
    {
        public string? Value { get; set; } = "";
        public FilterEnum? Filter { get; set; } = FilterEnum.None;

        public override string ToString()
        {
            return $"{Value},{Filter}";
        }
    }
}
