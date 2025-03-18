using FormSubmissionService.Data;
using FormSubmissionService.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace FormSubmissionService.Services
{
    public class FormService : IFormService
    {
        private readonly FormDbContext _context;

        public FormService(FormDbContext context)
        {
            _context = context;
        }

        public async Task<FormSubmission> CreateSubmissionAsync(FormSubmissionDto submissionDto)
        {
            var submission = new FormSubmission
            {
                FormType = submissionDto.FormType,
                FormData = JsonSerializer.Serialize(submissionDto.FormData),
                SubmittedAt = DateTime.UtcNow
            };

            _context.FormSubmissions.Add(submission);
            await _context.SaveChangesAsync();

            return submission;
        }

        public async Task<IEnumerable<FormSubmission>> GetAllSubmissionsAsync()
        {
            return await _context.FormSubmissions
                .OrderByDescending(s => s.SubmittedAt)
                .ToListAsync();
        }

        public async Task<FormSubmission?> GetSubmissionByIdAsync(int id)
        {
            return await _context.FormSubmissions.FindAsync(id);
        }

        public async Task<IEnumerable<FormSubmission>> SearchSubmissionsAsync(SearchRequest searchRequest)
        {
            var query = _context.FormSubmissions.AsQueryable();

            if (!string.IsNullOrEmpty(searchRequest.FormType))
            {
                query = query.Where(s => s.FormType == searchRequest.FormType);
            }
    
            if (searchRequest.FromDate.HasValue)
            {
                query = query.Where(s => s.SubmittedAt >= searchRequest.FromDate.Value);
            }

            if (searchRequest.ToDate.HasValue)
            {
                query = query.Where(s => s.SubmittedAt <= searchRequest.ToDate.Value);
            }

            if (!string.IsNullOrEmpty(searchRequest.SearchTerm))
            {
                query = query.Where(s => s.FormData.Contains(searchRequest.SearchTerm));
            }

            return await query
                .OrderByDescending(s => s.SubmittedAt)
                .Skip((searchRequest.Page - 1) * searchRequest.PageSize)
                .Take(searchRequest.PageSize)
                .ToListAsync();
        }
    }
} 