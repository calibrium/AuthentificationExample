using AuthentificationExample.Server.Abstractions;
using AuthentificationExample.Server.Exceptions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Models_LoginRequest = AuthentificationExample.Server.Models.LoginRequest;

namespace AuthentificationExample.Server.Services
{
    public class AuthenticationService(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IdentityService httpContextAccessor
        )
    {
        public async Task AuthenticateUser(Models_LoginRequest loginRequest)
        {
            var authClientDetails = await userRepository.FindUserAsync(loginRequest.Login);

            if (authClientDetails is null)
            {
                throw new UserNotFoundException();
            }

            if (authClientDetails.IsDeleted)
            {
                throw new AuthenticationException();
            }

            bool isPasswordValid = passwordHasher.Verify(
                loginRequest.Password, 
                authClientDetails.HashedPassword!, 
                authClientDetails.Salt!
            );

            if (!isPasswordValid)
            {
                throw new AuthenticationException();
            }

            await httpContextAccessor.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                loginRequest,
                authClientDetails
            );
        }

    }
}
