using FormSubmission.Core.Models;
using FormSubmissionService.DTO;

namespace FormSubmissionService.Services
{
    public interface IFormService
    {
        Task<Form> CreateFormAsync(FormCreateRequest formRequest);
        Task<IEnumerable<Form>> GetAllFormsAsync();
        Task<Form?> GetFormByIdAsync(int id);
        Task<IEnumerable<Form>> SearchFormsAsync(FormSearchRequest searchRequest);
    }
} 