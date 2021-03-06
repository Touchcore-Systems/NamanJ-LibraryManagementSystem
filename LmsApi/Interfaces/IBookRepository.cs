using LmsApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace LmsApi.Interfaces
{
    public interface IBookRepository
    {
        Task<IEnumerable<BookDetails>> GetBooksAsync();
        Task<string> AddBook(BookDetails bookDetails);
        Task<string> DeleteBook(int id, BookDetails bookDetails);
    }
}
