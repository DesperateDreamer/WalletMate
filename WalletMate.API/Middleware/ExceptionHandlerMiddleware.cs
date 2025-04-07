using System.Net;
using Newtonsoft.Json;
using WalletMate.BLL.Shared.CustomExceptions;

namespace WalletMate.API.Middleware;

public class ExceptionHandlerMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            response.StatusCode = ex switch
            {
                NotAcceptableException => (int)HttpStatusCode.BadRequest,
                NotFoundException => (int)HttpStatusCode.NotFound,
                UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
                _ => (int)HttpStatusCode.InternalServerError
            };

            var result = JsonConvert.SerializeObject(new { message = ex.Message });
            await response.WriteAsync(result);
        }
    }
}