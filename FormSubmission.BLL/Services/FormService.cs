using FormSubmission.BLL.DTO;
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

        public async Task<Form?> GetFormByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Form>> SearchFormsAsync(FormSearchRequest searchRequest)
        {
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