using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksLibrary.Core.Interfaces
{
    public interface ISeedService
    {
        Task Seed();
        Task Clear();
    }
}
