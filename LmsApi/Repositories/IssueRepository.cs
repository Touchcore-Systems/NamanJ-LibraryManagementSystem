using LmsApi.Data;
using LmsApi.Helpers;
using LmsApi.Interfaces;
using LmsApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LmsApi.Repositories
{
    public class IssueRepository : IIssueRepository
    {
        private readonly DataContext _context;

        LogRecordHelper logRecord = new();

        public IssueRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<JsonResult> GetIssueDetailsAsync()
        {
            try
            {
                var issueDetails = await _context.BookDetails
                    .Join(
                        _context.IssueDetails,
                        book => book.BId,
                        issue => issue.BId,
                        (book, issue) => new
                        {
                            TId = issue.TId,
                            UName = issue.UName,
                            BName = book.BName,
                            BQuantity = book.BQuantity,
                            DateOfIssue = issue.DateOfIssue,
                            DateOfSubmission = issue.DateofSubmission,
                            DateOfReturn = issue.DateOfReturn,
                            Status = issue.Status,
                            Fine = issue.Fine
                        }
                    ).ToListAsync();
                return new JsonResult(issueDetails);
            }
            catch (Exception ex)
            {
                logRecord.LogWriter(ex.ToString());
                return new JsonResult(ex.Message);
            }
        }

        public async Task<IssueDetails> IssueBook(IssueDetails issueBook)
        {
            try
            {
                await _context.IssueDetails.AddAsync(issueBook);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logRecord.LogWriter(ex.ToString());
            }
            return issueBook;
        }
    }
}
