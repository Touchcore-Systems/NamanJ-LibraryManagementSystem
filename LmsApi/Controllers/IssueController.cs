using LmsApi.Data;
using LmsApi.DTO;
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

        public IssueController(DataContext context, IIssueRepository issueRepository)
        {
            _context = context;
            _issueRepository = issueRepository;
        }

        // GET: api/Issue
        [HttpGet, Authorize(Roles = "admin")]
        public async Task<ActionResult> GetIssueDetails()
        {
            return await _issueRepository.GetIssueDetailsAsync();
        }

        // POST: api/Issue/request
        [HttpPost, Route("request"), Authorize(Roles = "admin, student")]
        public async Task<ActionResult<IssueDetails>> PostBookDetails(IssueDTO issueDto)
        {
            var issueDetails = new IssueDetails
            {
                UName = issueDto.UName,
                BId = issueDto.BId,
                Status = issueDto.Status
            };

            string res;
            try
            {
                await _issueRepository.IssueBook(issueDetails);
                res = "Book with BId " + issueDetails.BId + " requested";
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return new JsonResult(res);
        }
    }
}
