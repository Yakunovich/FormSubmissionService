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
            _logger.LogError(
                exception,
                "Error processing request {Method} {Path} from {IP}. TraceId: {TraceId}",
                context.Request.Method,
                context.Request.Path,
                context.Connection.RemoteIpAddress,
                context.TraceIdentifier
            );

            int statusCode = StatusCodes.Status500InternalServerError;
            string message = "An error occurred while processing your request.";
            IEnumerable<string>? errors = null;

            switch (exception)
            {
                case ValidationException validationEx:
                    statusCode = validationEx.StatusCode;
                    message = validationEx.Message;
                    errors = validationEx.Errors;
                    
                    _logger.LogWarning(
                        "Validation error: {Message}. Details: {@ValidationErrors}. TraceId: {TraceId}",
                        validationEx.Message,
                        validationEx.Errors,
                        context.TraceIdentifier
                    );
                    break;
                    
                case FormSubmissionException formEx:
                    statusCode = formEx.StatusCode;
                    message = formEx.Message;
                    
                    _logger.LogWarning(
                        "Business error ({StatusCode}): {Message}. TraceId: {TraceId}",
                        formEx.StatusCode,
                        formEx.Message,
                        context.TraceIdentifier
                    );
                    break;

                default:
                    if (_environment.IsDevelopment())
                    {
                        message = exception.Message;
                    }
                    
                    _logger.LogError(
                        exception,
                        "Unhandled exception: {ExceptionType}. TraceId: {TraceId}",
                        exception.GetType().Name,
                        context.TraceIdentifier
                    );
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