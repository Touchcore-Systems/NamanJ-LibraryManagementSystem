using LmsApi.Models;

namespace LmsApi.Interfaces
{
    public interface IBookRepository
    {
        Task<IEnumerable<BookDetails>> GetBooksAsync();
        Task<BookDetails> AddBook(BookDetails bookDetails);
    }
}
