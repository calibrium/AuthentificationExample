namespace AuthentificationExample.Server.Models
{
    public record StudentDTO(
        int UserId,
        string? FirstName,
        string? LastName,
        DateOnly Birthday,
        string? AliasFirstName,
        string? AliasLastName,
        string? Vk,
        string? WhatsApp,
        string? Tg,
        string? PhoneNumber,
        ParentDTO ParentDTO
    );
}
