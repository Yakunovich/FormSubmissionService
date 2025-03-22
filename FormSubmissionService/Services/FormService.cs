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

        public async Task<Form> CreateFormAsync(FormCreateRequest formRequest)
        {
            var form = new Form
            {
                FormType = formRequest.FormType,
                FormData = JsonSerializer.Serialize(formRequest.FormData),
                SubmittedAt = DateTime.UtcNow
            };

            _context.Forms.Add(form);
            await _context.SaveChangesAsync();

            return form;
        }

        public async Task<IEnumerable<Form>> GetAllFormsAsync()
        {
            return await _context.Forms
                .OrderByDescending(f => f.SubmittedAt)
                .ToListAsync();
        }

        public async Task<Form?> GetFormByIdAsync(int id)
        {
            return await _context.Forms.FindAsync(id);
        }

        public async Task<IEnumerable<Form>> SearchFormsAsync(FormSearchRequest searchRequest)
        {
            var query = _context.Forms.AsQueryable();

            if (!string.IsNullOrEmpty(searchRequest.FormType))
            {
                query = query.Where(f => f.FormType == searchRequest.FormType);
            }
    
            if (searchRequest.FromDate.HasValue)
            {
                query = query.Where(f => f.SubmittedAt >= searchRequest.FromDate.Value);
            }

            if (searchRequest.ToDate.HasValue)
            {
                query = query.Where(f => f.SubmittedAt <= searchRequest.ToDate.Value);
            }

            if (!string.IsNullOrEmpty(searchRequest.SearchTerm))
            {
                query = query.Where(f => f.FormData.Contains(searchRequest.SearchTerm));
            }

            return await query
                .OrderByDescending(f => f.SubmittedAt)
                .Skip((searchRequest.Page - 1) * searchRequest.PageSize)
                .Take(searchRequest.PageSize)
                .ToListAsync();
        }
    }
} 