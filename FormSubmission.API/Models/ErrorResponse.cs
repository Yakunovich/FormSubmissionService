using System.Text.Json.Serialization;

namespace FormSubmission.API.Models
{
    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public string TraceId { get; set; } = string.Empty;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IEnumerable<string>? Errors { get; set; }

        public ErrorResponse(int statusCode, string message, string? traceId = null, IEnumerable<string>? errors = null)
        {
            StatusCode = statusCode;
            Message = message;
            TraceId = traceId ?? Guid.NewGuid().ToString();
            Errors = errors;
        }
    }
} 