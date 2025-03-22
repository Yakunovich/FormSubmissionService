using FormSubmissionService.Models;
using Microsoft.EntityFrameworkCore;

namespace FormSubmissionService.Data
{
    public class FormDbContext : DbContext
    {
        public FormDbContext(DbContextOptions<FormDbContext> options) : base(options)
        {
        }

        public DbSet<Form> Forms { get; set; } = null!;
    }
} 