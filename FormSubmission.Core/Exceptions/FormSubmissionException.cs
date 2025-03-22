namespace FormSubmission.Core.Exceptions
{
    public class FormSubmissionException : Exception
    {
        public int StatusCode { get; }

        public FormSubmissionException(string message, int statusCode = 500) : base(message)
        {
            StatusCode = statusCode;
        }

        public FormSubmissionException(string message, Exception innerException, int statusCode = 500) 
            : base(message, innerException)
        {
            StatusCode = statusCode;
        }
    }
} 