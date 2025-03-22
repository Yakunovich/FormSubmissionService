namespace FormSubmission.BLL.DTO
{
    public class FormCreateRequest
    {
        public string FormType { get; set; } = string.Empty;
        public Dictionary<string, object> FormData { get; set; } = new Dictionary<string, object>();
    }
} 