namespace FormSubmission.Core.Exceptions
{
    public class ValidationException : FormSubmissionException
    {
        public IEnumerable<string> Errors { get; }

        public ValidationException(string message, IEnumerable<string>? errors = null)
            : base(message, 400)
        {
            Errors = errors ?? Enumerable.Empty<string>();
        }
    }
} 