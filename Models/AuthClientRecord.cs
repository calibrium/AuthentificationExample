namespace AuthentificationExample.Server.Models
{
    public record AuthClientRecord(
        int UserId,
        string? HashedPassword,
        string? Salt,
        RoleEnum RoleType,
        bool IsDeleted
    );
}
