using FormSubmission.Core.Models;
using FormSubmission.Core.Models.Parameters;

namespace FormSubmission.Core.Interfaces
{
    public interface IFormRepository
    {
        Task<Form> CreateAsync(Form form);
        Task<Form?> GetByIdAsync(int id);
        Task<IEnumerable<Form>> GetAllAsync();
        Task<IEnumerable<Form>> SearchAsync(FormSearchParameters parameters);
    }
} 