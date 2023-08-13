using BooksLibrary.Model;
using Microsoft.EntityFrameworkCore;

namespace BooksLibrary.Core.Services
{
    public class BaseService<T> where T : class
    {
        protected readonly BooksLibraryContext _context;
        protected DbSet<T> DbSet
        {
            get
            {
                try
                {
                    return _context.Set<T>();
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public BaseService(BooksLibraryContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            _context = context;
        }

        protected async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}
