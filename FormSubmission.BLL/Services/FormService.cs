using FormSubmission.BLL.DTO;
using FormSubmission.Core.Exceptions;
using FormSubmission.Core.Interfaces;
using FormSubmission.Core.Models;
using FormSubmission.Core.Models.Parameters;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace FormSubmission.BLL.Services
{
    public class FormService : IFormService
    {
        private readonly IFormRepository _repository;
        private readonly ILogger<FormService> _logger;

        public FormService(IFormRepository repository, ILogger<FormService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Form> CreateAsync(FormCreateRequest formRequest)
        {
            _logger.LogInformation("Creating form of type {FormType}", formRequest.FormType);
            
            if (string.IsNullOrEmpty(formRequest.FormType))
            {
                _logger.LogWarning("Attempt to create form with empty type");
                throw new BadRequestException("FormType is required");
            }

            var form = new Form
            {
                FormType = formRequest.FormType,
                FormData = JsonSerializer.Serialize(formRequest.FormData),
                SubmittedAt = DateTime.UtcNow
            };

            var result = await _repository.CreateAsync(form);
            _logger.LogInformation("Form created successfully with ID {FormId}", result.Id);
            
            return result;
        }

        public async Task<IEnumerable<Form>> GetAllAsync()
        {
            _logger.LogInformation("Getting all forms");
            var forms = await _repository.GetAllAsync();
            _logger.LogInformation("Retrieved {FormCount} forms", forms.Count());
            
            return forms;
        }

        public async Task<Form> GetByIdAsync(int id)
        {
            _logger.LogInformation("Requesting form with ID {FormId}", id);
            
            var form = await _repository.GetByIdAsync(id);
            
            if (form == null)
            {
                _logger.LogWarning("Form with ID {FormId} not found", id);
                throw new NotFoundException("Form", id);
            }
            
            _logger.LogInformation("Form with ID {FormId} found", id);
            return form;
        }

        public async Task<IEnumerable<Form>> SearchAsync(FormSearchRequest searchRequest)
        {
            _logger.LogInformation(
                "Searching forms with parameters: Type={FormType}, Search={SearchTerm}, Page={Page}, Size={PageSize}", 
                searchRequest.FormType, 
                searchRequest.SearchTerm,
                searchRequest.Page,
                searchRequest.PageSize
            );
            
            if (searchRequest.Page <= 0)
            {
                _logger.LogWarning("Invalid page number: {Page}", searchRequest.Page);
                throw new BadRequestException("Page must be greater than 0");
            }

            if (searchRequest.PageSize <= 0)
            {
                _logger.LogWarning("Invalid page size: {PageSize}", searchRequest.PageSize);
                throw new BadRequestException("PageSize must be greater than 0");
            }

            var parameters = new FormSearchParameters
            {
                FormType = searchRequest.FormType,
                SearchTerm = searchRequest.SearchTerm,
                FromDate = searchRequest.FromDate,
                ToDate = searchRequest.ToDate,
                Page = searchRequest.Page,
                PageSize = searchRequest.PageSize
            };

            var results = await _repository.SearchAsync(parameters);
            _logger.LogInformation("Found {ResultCount} forms", results.Count());
            
            return results;
        }
    }
} 