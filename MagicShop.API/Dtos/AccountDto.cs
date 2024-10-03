using MagicShop.Entity.Models;

namespace MagicShop.API.Dtos
{
    public record AccountDto(int Id, string Login, UserRole Role);
}
