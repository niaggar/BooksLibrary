using BooksLibrary.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BooksLibrary.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DBController : ControllerBase
    {
        private readonly BooksLibraryContext _context;

        public DBController(BooksLibraryContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult CheckDatabaseConnection()
        {
            _context.Database.EnsureCreated();
            return Ok("Db is SQL Server: " + _context.Database.IsSqlServer());
        }
    }
}
