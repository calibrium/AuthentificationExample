using Microsoft.AspNetCore.Diagnostics;
using System.Text.Json;
using AuthentificationExample.Server.Models;

namespace AuthentificationExample.Server.Exceptions
{
    public class CustomExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            httpContext.Response.StatusCode = exception switch
            {
                AuthenticationException => StatusCodes.Status401Unauthorized,
                UserNotFoundException => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError,
            };

            var jsonResponse = JsonSerializer.Serialize(
                new ExceptionDTO(
                    Message: exception.Message, 
                    ContentType: httpContext.Response.ContentType, 
                    StatusCode: httpContext.Response.StatusCode 
                )
            );

            await httpContext.Response.WriteAsync(jsonResponse, cancellationToken);

            return true;
        }
    }
}
