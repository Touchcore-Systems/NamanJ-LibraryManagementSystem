using LmsApi.Data;
using LmsApi.Helpers;
using LmsApi.Interfaces;
using LmsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LmsApi.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly DataContext _context;

        LogRecordHelper logRecord = new();
        public BookRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BookDetails>> GetBooksAsync()
        {
            return await _context.BookDetails.ToListAsync();
        }

        public async Task<string> AddBook(BookDetails bookDetails)
        {
            try
            {
                await _context.BookDetails.AddAsync(bookDetails);
                await _context.SaveChangesAsync();
                return "Book added";
            }
            catch (Exception ex)
            {
                logRecord.LogWriter(ex.ToString());
                return ex.Message;
            }
        }

        public async Task<string> DeleteBook(int id, BookDetails bookDetails)
        {
            try
            {
                _context.BookDetails.Remove(bookDetails);
                await _context.SaveChangesAsync();
                return "Book deleted";
            }
            catch (Exception ex)
            {
                logRecord.LogWriter(ex.ToString());
                return ex.Message;
            }
        }
    }
}
