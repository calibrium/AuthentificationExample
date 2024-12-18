namespace AuthentificationExample.Server.Abstractions
{
    public interface IPasswordHasher
    {
        bool Verify(string unHashedPassword, string hashedPassword, string salt);
    }
}
