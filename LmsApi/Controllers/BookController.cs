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

        LogRecordHelper logRecord = new();
        public BookController(DataContext context, IBookRepository bookRepository)
        {
            _context = context;
            _bookRepository = bookRepository;
        }

        // GET: api/Book
        [HttpGet, Authorize(Roles = "admin, student")]
        public async Task<ActionResult<IEnumerable<BookDetails>>> GetBooks()
        {
            try
            {
                var books = await _bookRepository.GetBooksAsync();
                logRecord.LogWriter("Books fetched");
                return Ok(books);
            }
            catch (Exception ex)
            {
                logRecord.LogWriter(ex.ToString());
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Book/5
        [HttpPut("{id}"), Authorize(Roles = "admin")]
        public async Task<IActionResult> PutBookDetails(int id, BookDetails bookDetails)
        {
            if (id != bookDetails.BId)
            {
                return BadRequest();
            }

            try
            {
                _context.Entry(bookDetails).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return new JsonResult("Book details updated");
            }
            catch (Exception ex)
            {
                logRecord.LogWriter(ex.ToString());
                return new JsonResult(ex.Message);
            }
        }

        // POST: api/Book
        [HttpPost, Authorize(Roles = "admin")]
        public async Task<ActionResult<BookDetails>> PostBookDetails(BookDTO bookDto)
        {
            try
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

                await _bookRepository.AddBook(bookDetails);
                return new JsonResult("Book added");
            }
            catch (Exception ex)
            {
                logRecord.LogWriter(ex.ToString());
                return new JsonResult(ex.Message);
            }
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

            try
            {
                return new JsonResult(await _bookRepository.DeleteBook(id, bookDetails));
            }
            catch (Exception ex)
            {
                logRecord.LogWriter(ex.ToString());
                return new JsonResult(ex.Message);
            }
        }
    }
}
