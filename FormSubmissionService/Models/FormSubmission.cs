using System.Text.Json;

namespace FormSubmissionService.Models
{
    public class FormSubmission
    {
        public int Id { get; set; }
        public string FormType { get; set; } = string.Empty;
        public string FormData { get; set; } = string.Empty;
        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

        public Dictionary<string, object> GetFormDataAsDictionary()
        {
            if (string.IsNullOrEmpty(FormData))
                return new Dictionary<string, object>();
                
            return JsonSerializer.Deserialize<Dictionary<string, object>>(FormData) 
                ?? new Dictionary<string, object>();
        }

        public void SetFormDataFromDictionary(Dictionary<string, object> data)
        {
            FormData = JsonSerializer.Serialize(data);
        }
    }
} 