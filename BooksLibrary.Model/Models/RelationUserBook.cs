using BooksLibrary.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksLibrary.Model.Models
{
    public class RelationUserBook
    {
        public int Id { get; set; }
        public RelationUserBookEnum Relation { get; set; }
        public int? Rating { get; set; }
        public string? Review { get; set; }
        public DateTime? Date { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }


        public virtual User User { get; set; }
        public virtual Book Book { get; set; }
    }
}
