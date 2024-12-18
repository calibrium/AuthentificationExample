using AuthentificationExample.Server.Models;

namespace AuthentificationExample.Server.Abstractions
{
    public interface IIdentityService
    {
        Task SignInAsync(string? scheme, LoginRequest loginRequest, AuthClientRecord authClientDetails);
    }
}
