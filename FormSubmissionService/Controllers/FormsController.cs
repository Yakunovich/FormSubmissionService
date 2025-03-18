using FormSubmissionService.Models;
using FormSubmissionService.Services;
using Microsoft.AspNetCore.Mvc;

namespace FormSubmissionService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FormsController : ControllerBase
    {
        private readonly IFormService _formService;

        public FormsController(IFormService formService)
        {
            _formService = formService;
        }

        [HttpPost]
        public async Task<IActionResult> SubmitForm([FromBody] FormSubmissionDto submissionDto)
        {
            if (string.IsNullOrEmpty(submissionDto.FormType))
            {
                return BadRequest("FormType is required");
            }

            var submission = await _formService.CreateSubmissionAsync(submissionDto);
            return CreatedAtAction(nameof(GetSubmission), new { id = submission.Id }, submission);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSubmissions()
        {
            var submissions = await _formService.GetAllSubmissionsAsync();
            return Ok(submissions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSubmission(int id)
        {
            var submission = await _formService.GetSubmissionByIdAsync(id);
            if (submission == null)
            {
                return NotFound();
            }

            return Ok(submission);
        }

        [HttpPost("search")]
        public async Task<IActionResult> SearchSubmissions([FromBody] SearchRequest searchRequest)
        {
            var submissions = await _formService.SearchSubmissionsAsync(searchRequest);
            return Ok(submissions);
        }
    }
} 