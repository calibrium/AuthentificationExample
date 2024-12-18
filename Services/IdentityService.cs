using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using AuthentificationExample.Server.Extensions;
using AuthentificationExample.Server.Abstractions;
using AuthentificationExample.Server.Models;

namespace AuthentificationExample.Server.Services
{
    public class IdentityService(IHttpContextAccessor httpContextAccessor, TimeProvider timeProvider, IConfiguration configuration) : IIdentityService
    {
        public async Task SignInAsync(string? scheme, Models.LoginRequest loginRequest, AuthClientRecord authClientDetails)
        {
            var claims = new HashSet<Claim>()
            {
                new(ClaimTypes.Name, loginRequest.Login),
                new(ClaimTypes.NameIdentifier, authClientDetails.UserId.ToString()),
                new(ClaimTypes.Role, authClientDetails.RoleType.ToString())
            };
            
            var claimsIdentity = new ClaimsIdentity(claims, scheme);

            var expiresUtc = timeProvider.GetUtcNow().AddMinutes(loginRequest.RememberMe 
                ? configuration.GetValue<int>("CookieSettings:MaxLifeCookies")
                : configuration.GetValue<int>("CookieSettings:MinLifeCookies")
            );

            var authProperties = new AuthenticationProperties()
            {
                IsPersistent = true,
                ExpiresUtc = expiresUtc
            };

            await httpContextAccessor.HttpContext!.SignInAsync(
                scheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties
            );

            var cookieOptions = new CookieOptions
            {
                HttpOnly = false,
                Secure = true,
                Expires = expiresUtc,
                SameSite = SameSiteMode.Strict
            };

            httpContextAccessor.HttpContext!.SetCookie(
                "user_role", 
                authClientDetails.RoleType.ToString(), 
                cookieOptions
            );
        }
    }
}
