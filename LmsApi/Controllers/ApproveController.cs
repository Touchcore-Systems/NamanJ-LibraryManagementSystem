using LmsApi.Data;
using LmsApi.DTO;
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

        public ApproveController(DataContext context, IApproveRepository approveRepository)
        {
            _context = context;
            _approveRepository = approveRepository;
        }

        // GET: api/Approve
        [HttpGet, Authorize(Roles = "admin, student")]
        public async Task<JsonResult> GetBooks()
        {
            var books = await _approveRepository.GetBooksAsync();
            return books;
        }

        // PUT: api/Approve/5
        [HttpPut("{id}"), Authorize(Roles = "admin, student")]
        public async Task<IActionResult> UpdateApproveStatus(int id, ApproveDTO approveDTO)
        {
            if (!TransactionDetailsExists(id))
            {
                return BadRequest();
            }

            try
            {
                await _approveRepository.ApproveBookAsync(id, approveDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return Ok(new JsonResult("Updated"));
        }

        private bool TransactionDetailsExists(int id)
        {
            return _context.IssueDetails.Any(e => e.TId == id);
        }
    }
}
