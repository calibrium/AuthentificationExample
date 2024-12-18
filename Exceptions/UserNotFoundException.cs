namespace AuthentificationExample.Server.Exceptions
{
    public class UserNotFoundException : Exception
    {
        const string message = "Authentication failed, incorrect login";
        public UserNotFoundException() : base(message) { }
    }
}
