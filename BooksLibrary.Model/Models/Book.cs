using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksLibrary.Model.Models
{
    public class Book
    {
        public int Id { get; set; }
        public int Pages { get; set; }
        public string Title { get; set; }
        public string Synopsis { get; set; }
        public string? CoverUrl { get; set; }
        public string? Isbn { get; set; }
        public string? Year { get; set; }
        public List<Genre> Genres { get; set; }
        public int AuthorId { get; set; }


        public virtual Author Author { get; set; }
        public virtual ICollection<RelationUserBook> RelationUsers { get; set; }
    }
}
