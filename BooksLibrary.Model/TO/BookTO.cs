using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksLibrary.Model.TO
{
    public class BookTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Synopsis { get; set; }
        public string CoverUrl { get; set; }
        public string Year { get; set; }
        public int Pages { get; set; }
        public string ISBN { get; set; }


        public GenreTO[] Genres { get; set; }
        public AuthorTO Author { get; set; }
    }
}
