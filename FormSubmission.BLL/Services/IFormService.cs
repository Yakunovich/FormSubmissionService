using FormSubmission.BLL.DTO;
using FormSubmission.Core.Models;

namespace FormSubmission.BLL.Services
{
    public interface IFormService
    {
        Task<Form> CreateFormAsync(FormCreateRequest formRequest);
        Task<IEnumerable<Form>> GetAllFormsAsync();
        Task<Form> GetFormByIdAsync(int id);
        Task<IEnumerable<Form>> SearchFormsAsync(FormSearchRequest searchRequest);
    }
} 