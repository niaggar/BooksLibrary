using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksLibrary.Model.TO
{
    public class PaginationTO
    {
        public int? Page { get; set; } = 1;
        public int? PageSize { get; set; } = 20;
    }
}
