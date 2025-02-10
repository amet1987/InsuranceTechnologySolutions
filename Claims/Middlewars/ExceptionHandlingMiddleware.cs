using Claims.Models;
using Microsoft.AspNetCore.Diagnostics;
using Shared.Exceptions;

namespace Claims.Middlewars
{
    public sealed class ExceptionHandlingMiddleware : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            (int statusCode, string title) = DetermineStatusCodeAndTitle(exception);

            var problemDetails = CreateProblemDetails(httpContext, exception, statusCode, title);

            if (exception is ValidationException validationException)
            {
                problemDetails.Errors = validationException.Errors;
            }

            httpContext.Response.StatusCode = statusCode;   
            await httpContext.Response.WriteAsJsonAsync(problemDetails);

            return true;
        }

        private static (int statusCode, string title) DetermineStatusCodeAndTitle(Exception exception)
        {
            return exception switch
            {
                ValidationException => (StatusCodes.Status400BadRequest, "Validation exception occured"),
                KeyNotFoundException => (StatusCodes.Status400BadRequest, "Object not found exception occured"),
                TimeoutException => (StatusCodes.Status400BadRequest, "A timeout occured"),
                _ => (StatusCodes.Status500InternalServerError, "An unholded error occured")
            };
        }

        private static CustomValidationProblemDetails CreateProblemDetails(HttpContext httpContext, Exception exception, int statusCode, string title)
        {
            return new CustomValidationProblemDetails
            {
                Status = statusCode,
                Type = exception.GetType().Name,
                Title = title,
                Detail = exception.Message
            };
        }
    }
}
