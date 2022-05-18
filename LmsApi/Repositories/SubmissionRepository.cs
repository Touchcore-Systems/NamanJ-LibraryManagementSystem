using LmsApi.Data;
using LmsApi.DTO;
using LmsApi.Interfaces;
using LmsApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LmsApi.Repositories
{
    public class SubmissionRepository : ISubmissionRepository
    {
        private readonly DataContext _context;
        public SubmissionRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<JsonResult> GetBooksAsync(string uName)
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
        public async Task<JsonResult> SubmitBookAsync(int id, SubmissionDTO submissionDTO)
        {
            IssueDetails details = _context.IssueDetails.Where(x => x.TId == id).FirstOrDefault();
            BookDetails bookDetails = _context.BookDetails.Where(x => x.BId == details.BId).FirstOrDefault();

            details.Status = submissionDTO.Status;
            details.DateOfReturn = TimeZoneInfo.ConvertTimeFromUtc(submissionDTO.DateOfReturn, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
            details.Fine = submissionDTO.Fine;
            bookDetails.BQuantity += 1;

            try
            {
                _context.Entry(details).State = EntityState.Modified;
                _context.Entry(bookDetails).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return new JsonResult("Updated");
        }
    }
}
