using FormSubmission.BLL.DTO;
using FormSubmission.BLL.Services;
using FormSubmission.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace FormSubmission.API.Controllers
{
    [ApiController]
    [Route("api/forms")]
    public class FormsController : ControllerBase
    {
        private readonly IFormService _formService;

        public FormsController(IFormService formService)
        {
            _formService = formService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] FormCreateRequest formRequest)
        {
            var form = await _formService.CreateAsync(formRequest);
            return Ok(form);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var forms = await _formService.GetAllAsync();
            return Ok(forms);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var form = await _formService.GetByIdAsync(id);
            return Ok(form);
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search([FromBody] FormSearchRequest searchRequest)
        {
            var forms = await _formService.SearchAsync(searchRequest);
            return Ok(forms);
        }
    }
} 