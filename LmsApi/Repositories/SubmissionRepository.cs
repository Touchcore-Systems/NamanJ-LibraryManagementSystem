using LmsApi.Data;
using LmsApi.DTO;
using LmsApi.Helpers;
using LmsApi.Interfaces;
using LmsApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LmsApi.Repositories
{
    public class SubmissionRepository : ISubmissionRepository
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;

        LogRecordHelper logRecord = new();
        public SubmissionRepository(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<JsonResult> GetBooksAsync(string uName)
        {
            try
            {
                var issueDetails = await _context.BookDetails
                    .Join(
                        _context.IssueDetails.Where(x => x.UName == uName && x.Status == "approved"),
                        book => book.BId,
                        issue => issue.BId,
                        (book, issue) => new
                        {
                            TId = issue.TId,
                            BName = book.BName,
                            BAuthor = book.BAuthor,
                            DateOfSubmission = issue.DateofSubmission
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
        public async Task<string> SubmitBookAsync(int id, SubmissionDTO submissionDTO)
        {
            try
            {
                IssueDetails details = _context.IssueDetails.Where(x => x.TId == id).FirstOrDefault();
                BookDetails bookDetails = _context.BookDetails.Where(x => x.BId == details.BId).FirstOrDefault();

                details.Status = submissionDTO.Status;
                details.DateOfReturn = submissionDTO.DateOfReturn;

                //calculating days b/w DateOfIssue and DateOfReturn
                var days = (details.DateOfReturn.Value.Date - details.DateOfIssue.Value.Date).TotalDays;
                _ = days > Int32.Parse(_configuration["AllowedDays"])
                        ? details.Fine = Int32.Parse(_configuration["FineAmount"]) * ((int)days - Int32.Parse(_configuration["AllowedDays"]))
                        : details.Fine = 0;
                bookDetails.BQuantity += 1;

                _context.Entry(details).State = EntityState.Modified;
                _context.Entry(bookDetails).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return "Book submitted";
            }
            catch (Exception ex)
            {
                logRecord.LogWriter(ex.ToString());
                return ex.Message;
            }
        }
    }
}
