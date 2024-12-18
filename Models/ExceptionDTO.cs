namespace AuthentificationExample.Server.Models
{
    public record ExceptionDTO(
        int StatusCode,
        string? Message,
        string? ContentType
    );
}
