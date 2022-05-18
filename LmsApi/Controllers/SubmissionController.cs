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

        LogRecord logRecord = new();

        public SubmissionController(DataContext context, ISubmissionRepository submissionRepository)
        {
            _context = context;
            _submissionRepository = submissionRepository;
        }

        // GET: api/Submission
        [HttpGet, Authorize(Roles = "admin, student")]
        public async Task<JsonResult> GetStudentBooks()
        {
            var books = await _submissionRepository.GetBooksAsync(User.Identity.Name);
            return books;
        }

        // PUT: api/Submission/5
        [HttpPut("{id}"), Authorize(Roles = "admin, student")]
        public async Task<IActionResult> UpdateApproveStatus(int id, SubmissionDTO submissionDTO)
        {
            if (!TransactionDetailsExists(id))
            {
                return BadRequest();
            }

            string res = string.Empty;
            try
            {
                await _submissionRepository.SubmitBookAsync(id, submissionDTO);
                res = "Book approved";
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
