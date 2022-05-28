using LmsApi.DTO;
using Microsoft.AspNetCore.Mvc;

namespace LmsApi.Interfaces
{
    public interface ISubmissionRepository
    {
        Task<JsonResult> GetBooksAsync(string uName);
        Task<string> SubmitBookAsync(int id, SubmissionDTO submissionDTO);
    }
}
