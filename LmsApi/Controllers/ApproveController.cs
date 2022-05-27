using LmsApi.Data;
using LmsApi.DTO;
using LmsApi.Helpers;
using LmsApi.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LmsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApproveController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IApproveRepository _approveRepository;

        LogRecord logRecord = new();

        public ApproveController(DataContext context, IApproveRepository approveRepository)
        {
            _context = context;
            _approveRepository = approveRepository;
        }

        // GET: api/Approve
        [HttpGet, Authorize(Roles = "admin, student")]
        public async Task<JsonResult> GetBooks()
        {
            string res = String.Empty;

            try
            {
                var books = await _approveRepository.GetBooksAsync();
                res = "Books to approve fetched";
                return books;
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            finally
            {
                logRecord.LogWriter(res);
            }
            return new JsonResult(res);
        }

        // PUT: api/Approve/5
        [HttpPut("{id}"), Authorize(Roles = "admin, student")]
        public async Task<IActionResult> UpdateApproveStatus(int id, ApproveDTO approveDTO)
        {
            if (!TransactionDetailsExists(id))
            {
                return BadRequest();
            }

            string res = String.Empty;
            try
            {
                await _approveRepository.ApproveBookAsync(id, approveDTO);
                res = "book approved";
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            finally
            {
                logRecord.LogWriter(res);
            }

            return new JsonResult(res);
        }

        private bool TransactionDetailsExists(int id)
        {
            return _context.IssueDetails.Any(e => e.TId == id);
        }
    }
}
