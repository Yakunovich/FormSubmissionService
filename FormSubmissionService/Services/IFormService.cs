using FormSubmissionService.Models;

namespace FormSubmissionService.Services
{
    public interface IFormService
    {
        Task<FormSubmission> CreateSubmissionAsync(FormSubmissionDto submissionDto);
        Task<IEnumerable<FormSubmission>> GetAllSubmissionsAsync();
        Task<FormSubmission?> GetSubmissionByIdAsync(int id);
        Task<IEnumerable<FormSubmission>> SearchSubmissionsAsync(SearchRequest searchRequest);
    }
} 