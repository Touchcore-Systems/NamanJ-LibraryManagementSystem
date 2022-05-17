using LmsApi.DTO;
using Microsoft.AspNetCore.Mvc;

namespace LmsApi.Interfaces
{
    public interface ISubmissionRepository
    {
        Task<JsonResult> GetBooksAsync(string uName);
        Task<JsonResult> SubmitBookAsync(int id, SubmissionDTO submissionDTO);
    }
}
