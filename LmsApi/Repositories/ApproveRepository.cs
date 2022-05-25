using LmsApi.Data;
using LmsApi.DTO;
using LmsApi.Interfaces;
using LmsApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LmsApi.Repositories
{
    public class ApproveRepository : IApproveRepository
    {
        private readonly DataContext _context;
        public ApproveRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<JsonResult> GetBooksAsync()
        {
            var issueDetails = await _context.BookDetails
                .Join(
                        _context.IssueDetails.Where(x => x.Status == "pending"),
                        book => book.BId,
                        issue => issue.BId,
                        (book, issue) => new
                        {
                            TId = issue.TId,
                            UName = issue.UName,
                            BId = book.BId,
                            BName = book.BName,
                            BAuthor = book.BAuthor,
                            BQuantity = book.BQuantity
                        }
                    ).ToListAsync();
            return new JsonResult(issueDetails);
        }

        public async Task<JsonResult> ApproveBookAsync(int id, ApproveDTO approveDTO)
        {
            IssueDetails details = _context.IssueDetails.Where(x => x.TId == id).FirstOrDefault();
            BookDetails bookDetails = _context.BookDetails.Where(x => x.BId == details.BId).FirstOrDefault();

            if (bookDetails?.BQuantity == 0)
            {
                return new JsonResult("Out of Stock!");
            }

            details.Status = approveDTO.Status;
            details.DateOfIssue = approveDTO.DateOfIssue;
            details.DateofSubmission = approveDTO.DateOfSubmission;
            bookDetails.BQuantity = bookDetails.BQuantity - 1;

            try
            {
                _context.Entry(details).State = EntityState.Modified;
                _context.Entry(bookDetails).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return new JsonResult("Updated");
        }
    }
}
