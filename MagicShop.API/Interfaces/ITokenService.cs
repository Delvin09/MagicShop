using MagicShop.Entity.Models;

namespace MagicShop.API.Interfaces
{
    public interface ITokenService
    {
        public string GetToken(UserAccount account);
    }
}
