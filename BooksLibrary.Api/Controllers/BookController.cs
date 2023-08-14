using BooksLibrary.Core.Interfaces;
using BooksLibrary.Model.Enums;
using BooksLibrary.Model.TO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BooksLibrary.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BookController : ControllerBase
    {
        private readonly IBookService _service;

        public BookController(IBookService service)
        {
            _service = service;
        }

        [HttpGet("Get", Name = "GetBook")]
        public async Task<IActionResult> Get([FromQuery] int id)
        {
            try
            {
                var book = await _service.GetBook(id);
                var res = new ResultTO<BookTO> { Data = book, Success = true };

                return Ok(res);
            }
            catch (Exception ex)
            {
                var res = new ResultTO<object> { Success = false, Message = ex.Message };
                return StatusCode((int)HttpStatusCode.InternalServerError, res);
            }
        }

        [HttpGet("GetAll/{filter.Filter?}/{filter.Value?}", Name = "GetBooks")]
        public async Task<IActionResult> Get([FromRoute] FilterTO? filter, [FromQuery] PaginationTO? pagination)
        {
            try
            {
                var books = await _service.GetBooks(filter, pagination);
                var res = new ResultTO<PaginationResultTO<BookTO>> { Data = books, Success = true };

                return Ok(res);
            }
            catch (Exception ex)
            {
                var res = new ResultTO<object> { Success = false, Message = ex.Message };
                return StatusCode((int)HttpStatusCode.InternalServerError, res);
            }
        }

        [HttpGet("GetBookWithUserRelation/{relation?}", Name = "GetBookWithUserRelation")]
        public async Task<IActionResult> GetBookWithUserRelation([FromQuery] int id, [FromRoute] RelationUserBookEnum? relation)
        {
            try
            {
                var book = await _service.GetBooksByUser(id, relation);
                var res = new ResultTO<IEnumerable<BookTO>> { Data = book, Success = true };

                return Ok(res);
            }
            catch (Exception ex)
            {
                var res = new ResultTO<object> { Success = false, Message = ex.Message };
                return StatusCode((int)HttpStatusCode.InternalServerError, res);
            }
        }
    }
}
