using System.Net;
using Newtonsoft.Json;
using WalletMate.Application.Exceptions;

namespace WalletMate.Adapters.In.API.Middleware;

// Chain of the Responsibility pattern
public class ExceptionHandlerMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ApplicationExceptionBase ex)
        {
            await HandleApplicationException(context, ex);
        }
        catch (Exception)
        {
            await HandleUnexpectedException(context);
        }
    }

    private static async Task HandleApplicationException(
        HttpContext context,
        ApplicationExceptionBase exception)
    {
        var (statusCode, error) = exception switch
        {
            EntityNotFoundException ex => (HttpStatusCode.NotFound,
                    ErrorResponse.From(ex.Message)),

            ValidationException ex => (HttpStatusCode.BadRequest,
                    ErrorResponse.From(ex.Errors)),

            BusinessRuleViolationException ex => (HttpStatusCode.Conflict,
                    ErrorResponse.From(ex.Message)),

            ConflictException ex => (HttpStatusCode.Conflict,
                    ErrorResponse.From(ex.Message)),

            ForbiddenOperationException ex => (HttpStatusCode.Forbidden,
                    ErrorResponse.From(ex.Message)),

            _ => (HttpStatusCode.BadRequest,
                    ErrorResponse.From(exception.Message))
        };

        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";

        var json = JsonConvert.SerializeObject(error);
        await context.Response.WriteAsync(json);
    }

    private static async Task HandleUnexpectedException(HttpContext context)
    {
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Response.ContentType = "application/json";

        var error = ErrorResponse.From("An unexpected error occurred.");
        var json = JsonConvert.SerializeObject(error);

        await context.Response.WriteAsync(json);
    }
}