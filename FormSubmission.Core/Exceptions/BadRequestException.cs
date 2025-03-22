namespace FormSubmission.Core.Exceptions
{
    public class BadRequestException : FormSubmissionException
    {
        public BadRequestException(string message) : base(message, 400)
        {
        }
    }
} 