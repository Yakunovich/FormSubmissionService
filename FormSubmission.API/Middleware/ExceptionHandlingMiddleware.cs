using FormSubmission.API.Models;
using FormSubmission.Core.Exceptions;
using System.Text.Json;

namespace FormSubmission.API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IWebHostEnvironment _environment;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger,
            IWebHostEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _environment = environment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogError(exception, "An unhandled exception occurred.");

            int statusCode = StatusCodes.Status500InternalServerError;
            string message = "An error occurred while processing your request.";
            IEnumerable<string>? errors = null;

            switch (exception)
            {
                case ValidationException validationEx:
                    statusCode = validationEx.StatusCode;
                    message = validationEx.Message;
                    errors = validationEx.Errors;
                    break;
                    
                case FormSubmissionException formEx:
                    statusCode = formEx.StatusCode;
                    message = formEx.Message;
                    break;

                default:
                    if (_environment.IsDevelopment())
                    {
                        message = exception.Message;
                    }
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var errorResponse = new ErrorResponse(
                statusCode,
                message,
                context.TraceIdentifier,
                errors
            );

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse, jsonOptions));
        }
    }
} 