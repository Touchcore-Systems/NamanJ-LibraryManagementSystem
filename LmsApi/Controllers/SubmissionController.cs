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
    public class SubmissionController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly ISubmissionRepository _submissionRepository;

        LogRecordHelper logRecord = new();

        public SubmissionController(DataContext context, ISubmissionRepository submissionRepository)
        {
            _context = context;
            _submissionRepository = submissionRepository;
        }

        // GET: api/Submission
        [HttpGet, Authorize(Roles = "admin, student")]
        public async Task<IActionResult> GetStudentBooks()
        {
            try
            {
                logRecord.LogWriter("Books issued to a student fetched");
                return await _submissionRepository.GetBooksAsync(User.Identity.Name);
            }
            catch (Exception ex)
            {
                logRecord.LogWriter(ex.ToString());
                return NotFound(ex.Message);
            }
        }

        // PUT: api/Submission/5
        [HttpPut("{id}"), Authorize(Roles = "admin, student")]
        public async Task<IActionResult> UpdateApproveStatus(int id, SubmissionDTO submissionDTO)
        {
            if (!TransactionDetailsExists(id))
            {
                return BadRequest();
            }

            try
            {
                await _submissionRepository.SubmitBookAsync(id, submissionDTO);
                return new JsonResult("Book submitted");
            }
            catch (Exception ex)
            {
                logRecord.LogWriter(ex.ToString());
                return BadRequest(ex.Message);
            }
        }
        private bool TransactionDetailsExists(int id)
        {
            return _context.IssueDetails.Any(e => e.TId == id);
        }
    }
}
