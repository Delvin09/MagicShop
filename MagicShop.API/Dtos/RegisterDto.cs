namespace MagicShop.API.Dtos
{
    public record RegisterDto(string FirstName,
        string LastName,
        string? Email,
        string? Phone,
        string Login,
        string Password);
}
