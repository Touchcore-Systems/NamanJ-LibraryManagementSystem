using LmsApi.Data;
using LmsApi.DTO;
using LmsApi.Helpers;
using LmsApi.Interfaces;
using LmsApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LmsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IssueController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IIssueRepository _issueRepository;

        LogRecordHelper logRecord = new();

        public IssueController(DataContext context, IIssueRepository issueRepository)
        {
            _context = context;
            _issueRepository = issueRepository;
        }

        // GET: api/Issue
        [HttpGet, Authorize(Roles = "admin")]
        public async Task<IActionResult> GetIssueDetails()
        {
            JsonResult issueDetails;

            try
            {
                issueDetails = await _issueRepository.GetIssueDetailsAsync();
                logRecord.LogWriter("Issue details fetched");
                return issueDetails;
            }
            catch (Exception ex)
            {
                logRecord.LogWriter(ex.ToString());
                return BadRequest(ex.Message);
            }
        }

        // POST: api/Issue/request
        [HttpPost, Route("request"), Authorize(Roles = "admin, student")]
        public async Task<IActionResult> PostBookDetails(IssueDTO issueDto)
        {
            try
            {
                var issueDetails = new IssueDetails
                {
                    UName = issueDto.UName,
                    BId = issueDto.BId,
                    Status = issueDto.Status
                };

                await _issueRepository.IssueBook(issueDetails);
                return new JsonResult("Book requested");
            }
            catch (Exception ex)
            {
                logRecord.LogWriter(ex.ToString());
                return new JsonResult(ex.Message);
            }
        }
    }
}
