using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksLibrary.Model.TO
{
    public class PaginationResultTO<T>
    {
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public IEnumerable<T> Items { get; set; }
        public PaginationTO? Pagination { get; set; }
    }
}
