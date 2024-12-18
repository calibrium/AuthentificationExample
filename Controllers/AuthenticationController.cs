using AuthentificationExample.Server.Constants;
using AuthentificationExample.Server.Models;
using AuthentificationExample.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace AuthentificationExample.Server.Controllers
{
    
    [Route(ApiRoutes.Project.Base)]
    public class AuthenticationController(AuthenticationService authenticationService) : ControllerBase
    {
        [HttpPost(ApiRoutes.Project.Login)]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            await authenticationService.AuthenticateUser(loginRequest);
            return Ok();
        }
    }
}
