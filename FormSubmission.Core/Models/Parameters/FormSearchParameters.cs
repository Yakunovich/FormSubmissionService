using FormSubmission.Core.Constants;

namespace FormSubmission.Core.Models.Parameters
{
    public class FormSearchParameters
    {
        public string? FormType { get; set; }
        public string? SearchTerm { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int Page { get; set; } = SearchConstants.Pagination.DefaultPage;
        public int PageSize { get; set; } = SearchConstants.Pagination.DefaultPageSize;
    }
} 