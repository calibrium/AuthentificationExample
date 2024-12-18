namespace AuthentificationExample.Server.Exceptions
{
    public class AuthenticationException : Exception
    {
        const string message = "Authentication failed, user deleted";
        public AuthenticationException() : base(message) { }
    }
}
