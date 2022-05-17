using LmsApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace LmsApi.Interfaces
{
    public interface IIssueRepository
    {
        Task<JsonResult> GetIssueDetailsAsync();
        Task<IssueDetails> IssueBook(IssueDetails issueBook);
    }
}
