using FormSubmission.Core.Interfaces;
using FormSubmission.Core.Models;
using FormSubmission.Core.Models.Parameters;
using FormSubmission.DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace FormSubmission.DAL.Repositories
{
    public class FormRepository : IFormRepository
    {
        private readonly FormDbContext _context;

        public FormRepository(FormDbContext context)
        {
            _context = context;
        }

        public async Task<Form> CreateAsync(Form form)
        {
            _context.Forms.Add(form);
            await _context.SaveChangesAsync();
            return form;
        }

        public async Task<IEnumerable<Form>> GetAllAsync()
        {
            return await _context.Forms
                .OrderByDescending(f => f.SubmittedAt)
                .ToListAsync();
        }

        public async Task<Form?> GetByIdAsync(int id)
        {
            return await _context.Forms.FindAsync(id);
        }

        public async Task<IEnumerable<Form>> SearchAsync(FormSearchParameters parameters)
        {
            var query = _context.Forms.AsQueryable();

            if (!string.IsNullOrEmpty(parameters.FormType))
            {
                query = query.Where(f => f.FormType == parameters.FormType);
            }
    
            if (parameters.FromDate.HasValue)
            {
                query = query.Where(f => f.SubmittedAt >= parameters.FromDate.Value);
            }

            if (parameters.ToDate.HasValue)
            {
                query = query.Where(f => f.SubmittedAt <= parameters.ToDate.Value);
            }

            if (!string.IsNullOrEmpty(parameters.SearchTerm))
            {
                query = query.Where(f => f.FormData.Contains(parameters.SearchTerm));
            }

            return await query
                .OrderByDescending(f => f.SubmittedAt)
                .Skip((parameters.Page - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();
        }
    }
} 