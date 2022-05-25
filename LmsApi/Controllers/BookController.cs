#nullable disable
using LmsApi.Data;
using LmsApi.DTO;
using LmsApi.Helpers;
using LmsApi.Interfaces;
using LmsApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LmsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IBookRepository _bookRepository;

        LogRecord logRecord = new();
        public BookController(DataContext context, IBookRepository bookRepository)
        {
            _context = context;
            _bookRepository = bookRepository;
        }

        // GET: api/Book
        [HttpGet, Authorize(Roles = "admin, student")]
        public async Task<ActionResult<IEnumerable<BookDetails>>> GetBooks()
        {
            string res = string.Empty;
            try
            {
                var books = await _bookRepository.GetBooksAsync();
                res = "Books fetched";
                return Ok(books);
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            finally
            {
                logRecord.LogWriter(res);
            }
            return BadRequest(res);
        }

        // PUT: api/Book/5
        [HttpPut("{id}"), Authorize(Roles = "admin")]
        public async Task<IActionResult> PutBookDetails(int id, BookDetails bookDetails)
        {
            if (id != bookDetails.BId)
            {
                return BadRequest();
            }

            _context.Entry(bookDetails).State = EntityState.Modified;

            string res = string.Empty;
            try
            {
                await _context.SaveChangesAsync();
                res = "Book details updated";
                return new JsonResult(res);
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            finally
            {
                logRecord.LogWriter(res);
            }
            return new JsonResult(res);
        }

        // POST: api/Book
        [HttpPost, Authorize(Roles = "admin")]
        public async Task<ActionResult<BookDetails>> PostBookDetails(BookDTO bookDto)
        {
            var bookExists = _context.BookDetails.Any(x => x.BName == bookDto.BName && x.BAuthor == bookDto.BAuthor);
            if (bookExists)
            {
                return new JsonResult("Book already exists");
            }
            var bookDetails = new BookDetails
            {
                BName = bookDto.BName,
                BAuthor = bookDto.BAuthor,
                BQuantity = bookDto.BQuantity
            };

            string res = string.Empty;
            try
            {
                await _bookRepository.AddBook(bookDetails);
                res = "Book added";
                return new JsonResult(res);
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            finally
            {
                logRecord.LogWriter(res);
            }
            return new JsonResult(res);
        }

        // DELETE: api/Book/5
        [HttpDelete("{id}"), Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteBookDetails(int id)
        {
            var bookDetails = await _context.BookDetails.FindAsync(id);
            if (bookDetails == null)
            {
                return NotFound();
            }

            var res = string.Empty;
            try
            {
                res = await _bookRepository.DeleteBook(id, bookDetails);
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            finally
            {
                logRecord.LogWriter(res);
            }

            return new JsonResult(res);
        }

        /*
        private bool BookDetailsExists(int id)
        {
            return _context.BookDetails.Any(e => e.BId == id);
        }
        */
    }
}
