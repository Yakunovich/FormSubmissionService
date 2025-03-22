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
        public async Task<IActionResult> CreateForm([FromBody] FormCreateRequest formRequest)
        {
            if (string.IsNullOrEmpty(formRequest.FormType))
            {
                return BadRequest("FormType is required");
            }

            var form = await _formService.CreateFormAsync(formRequest);
            return CreatedAtAction(nameof(GetForm), new { id = form.Id }, form);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllForms()
        {
            var forms = await _formService.GetAllFormsAsync();
            return Ok(forms);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetForm(int id)
        {
            var form = await _formService.GetFormByIdAsync(id);
            if (form == null)
            {
                return NotFound();
            }

            return Ok(form);
        }

        [HttpPost("search")]
        public async Task<IActionResult> SearchForms([FromBody] FormSearchRequest searchRequest)
        {
            var forms = await _formService.SearchFormsAsync(searchRequest);
            return Ok(forms);
        }
    }
} 