namespace AuthentificationExample.Server.Models
{
    public record ParentDTO(
        string? FirstName,
        string? MiddleName,
        string? LastName,
        string? Vk,
        string? WhatsApp,
        string? Tg,
        string? PhoneNumber
    );
}
