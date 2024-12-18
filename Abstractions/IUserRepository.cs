using AuthentificationExample.Server.Models;

namespace AuthentificationExample.Server.Abstractions
{
    public interface IUserRepository
    {
        Task<AuthClientRecord?> FindUserAsync(string login);
        IAsyncEnumerable<StudentDTO> GetUsersByIdAsync(int id);
        Task UpdateUserNameAsync(int userId, string aliasFirstName, string aliasLastName);
    }
}
