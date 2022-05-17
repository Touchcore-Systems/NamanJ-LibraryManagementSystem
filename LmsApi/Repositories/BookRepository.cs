using LmsApi.Data;
using LmsApi.Interfaces;
using LmsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LmsApi.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly DataContext _context;
        public BookRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BookDetails>> GetBooksAsync()
        {
            return await _context.BookDetails.ToListAsync();
        }

        public async Task<BookDetails> AddBook(BookDetails bookDetails)
        {
            try
            {
                await _context.BookDetails.AddAsync(bookDetails);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return bookDetails;
        }
    }
}
