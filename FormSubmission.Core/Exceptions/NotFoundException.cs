namespace FormSubmission.Core.Exceptions
{
    public class NotFoundException : FormSubmissionException
    {
        public NotFoundException(string message) : base(message, 404)
        {
        }

        public NotFoundException(string entityName, object key)
            : base($"{entityName} with id {key} was not found.", 404)
        {
        }
    }
} 