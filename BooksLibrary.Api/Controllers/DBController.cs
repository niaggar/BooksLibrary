using BooksLibrary.Core.Interfaces;
using BooksLibrary.Core.Services;
using BooksLibrary.Model;
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

        [HttpGet("Check")]
        public async Task<IActionResult> CheckDatabase()
        {
            await _context.Database.EnsureDeletedAsync();
            await _context.Database.EnsureCreatedAsync();

            return Ok("Db is SQL Server: " + _context.Database.IsSqlServer());
        }

        [HttpGet("Seed")]
        public async Task<IActionResult> SeedDatabase()
        {
            try
            {
                var seedService = new SeedService(_context) as ISeedService;

                await seedService.Seed();
                return Ok(true);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
