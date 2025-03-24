using FormSubmission.BLL.DTO;
using FormSubmission.Core.Models;

namespace FormSubmission.BLL.Services
{
    public interface IFormService
    {
        Task<Form> CreateAsync(FormCreateRequest formRequest);
        Task<IEnumerable<Form>> GetAllAsync();
        Task<Form> GetByIdAsync(int id);
        Task<IEnumerable<Form>> SearchAsync(FormSearchRequest searchRequest);
    }
} 