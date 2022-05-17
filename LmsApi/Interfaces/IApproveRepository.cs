using LmsApi.DTO;
using LmsApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace LmsApi.Interfaces
{
    public interface IApproveRepository
    {
        Task<JsonResult> GetBooksAsync();
        Task<JsonResult> ApproveBookAsync(int id, ApproveDTO approveDTO);
    }
}
