using FormSubmission.BLL.DTO;
using FormSubmission.Core.Exceptions;
using FormSubmission.Core.Interfaces;
using FormSubmission.Core.Models;
using FormSubmission.Core.Models.Parameters;
using System.Text.Json;

namespace FormSubmission.BLL.Services
{
    public class FormService : IFormService
    {
        private readonly IFormRepository _repository;

        public FormService(IFormRepository repository)
        {
            _repository = repository;
        }

        public async Task<Form> CreateFormAsync(FormCreateRequest formRequest)
        {
            if (string.IsNullOrEmpty(formRequest.FormType))
            {
                throw new BadRequestException("FormType is required");
            }

            var form = new Form
            {
                FormType = formRequest.FormType,
                FormData = JsonSerializer.Serialize(formRequest.FormData),
                SubmittedAt = DateTime.UtcNow
            };

            return await _repository.CreateAsync(form);
        }

        public async Task<IEnumerable<Form>> GetAllFormsAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Form> GetFormByIdAsync(int id)
        {
            var form = await _repository.GetByIdAsync(id);
            
            if (form == null)
            {
                throw new NotFoundException("Form", id);
            }
            
            return form;
        }

        public async Task<IEnumerable<Form>> SearchFormsAsync(FormSearchRequest searchRequest)
        {
            if (searchRequest.Page <= 0)
            {
                throw new BadRequestException("Page must be greater than 0");
            }

            if (searchRequest.PageSize <= 0)
            {
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

            return await _repository.SearchAsync(parameters);
        }
    }
} 