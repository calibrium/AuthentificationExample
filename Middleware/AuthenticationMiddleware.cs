using Microsoft.AspNetCore.Authentication;

namespace AuthentificationExample.Server.Middleware
{
    public class AuthenticationMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext httpContext)
        {

            string? path = httpContext.Request.Path.Value;

            if (path!.StartsWith("/api/TutorSphere/login"))
            {
                await _next(httpContext);
                return;
            }

            var authenticateResult = await httpContext.AuthenticateAsync();

            if (!authenticateResult.Succeeded)
            {
                var errorResponseModel = new { Message = "User unauthorized" };
                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await httpContext.Response.WriteAsJsonAsync(errorResponseModel);
                return;
            }

            await _next(httpContext);
        }
    }
}
