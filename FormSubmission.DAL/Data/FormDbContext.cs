using FormSubmission.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace FormSubmission.DAL.Data
{
    public class FormDbContext : DbContext
    {
        public FormDbContext(DbContextOptions<FormDbContext> options) : base(options)
        {
        }

        public DbSet<Form> Forms { get; set; } = null!;
    }
} 