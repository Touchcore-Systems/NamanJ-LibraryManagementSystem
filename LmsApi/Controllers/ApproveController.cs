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

        LogRecordHelper logRecord = new();

        public ApproveController(DataContext context, IApproveRepository approveRepository)
        {
            _context = context;
            _approveRepository = approveRepository;
        }

        // GET: api/Approve
        [HttpGet, Authorize(Roles = "admin")]
        public async Task<JsonResult> GetBooks()
        {
            try
            {
                logRecord.LogWriter("Books to approve fetched");
                return await _approveRepository.GetBooksAsync();
            }
            catch (Exception ex)
            {
                logRecord.LogWriter(ex.ToString());
                return new JsonResult(ex.Message);
            }
        }

        // PUT: api/Approve/5
        [HttpPut("{id}"), Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateApproveStatus(int id, ApproveDTO approveDTO)
        {
            if (!TransactionDetailsExists(id))
            {
                return BadRequest();
            }

            try
            {
                await _approveRepository.ApproveBookAsync(id, approveDTO);
                return new JsonResult("Book approved");
            }
            catch (Exception ex)
            {
                logRecord.LogWriter(ex.ToString());
                return new JsonResult(ex.Message);
            }
        }

        private bool TransactionDetailsExists(int id)
        {
            return _context.IssueDetails.Any(e => e.TId == id);
        }
    }
}
