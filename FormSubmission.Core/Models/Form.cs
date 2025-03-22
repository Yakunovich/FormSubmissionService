namespace FormSubmission.Core.Models
{
    public class Form
    {
        public int Id { get; set; }
        public string FormType { get; set; } = string.Empty;
        public string FormData { get; set; } = string.Empty;    
        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
    }
} 