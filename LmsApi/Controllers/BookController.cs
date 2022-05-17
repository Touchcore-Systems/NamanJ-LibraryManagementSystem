#nullable disable
using LmsApi.Data;
using LmsApi.DTO;
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

        public BookController(DataContext context, IBookRepository bookRepository)
        {
            _context = context;
            _bookRepository = bookRepository;
        }

        // GET: api/Book
        [HttpGet, Authorize(Roles = "admin, student")]
        public async Task<ActionResult<IEnumerable<BookDetails>>> GetBooks()
        {
            var books = await _bookRepository.GetBooksAsync();
            return Ok(books);
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

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookDetailsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            //return NoContent();
            return new JsonResult("Updated");
        }

        // POST: api/Book
        [HttpPost, Authorize(Roles = "admin")]
        public async Task<ActionResult<BookDetails>> PostBookDetails(BookDTO bookDto)
        {
            var bookDetails = new BookDetails
            {
                BName = bookDto.BName,
                BAuthor = bookDto.BAuthor,
                BQuantity = bookDto.BQuantity
            };

            string res;
            try
            {
                //var book = await _bookRepository.AddBook(bookDetails);
                await _bookRepository.AddBook(bookDetails);
                res = "Book added";
            }
            catch (Exception ex)
            {
                res = ex.Message;
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

            _context.BookDetails.Remove(bookDetails);
            await _context.SaveChangesAsync();

            //return NoContent();
            return new JsonResult("Book deleted");
        }

        private bool BookDetailsExists(int id)
        {
            return _context.BookDetails.Any(e => e.BId == id);
        }
    }
}
