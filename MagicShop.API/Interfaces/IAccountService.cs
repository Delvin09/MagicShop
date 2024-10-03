using MagicShop.API.Dtos;
using MagicShop.Entity.Models;
using System.Threading;

namespace MagicShop.API.Interfaces
{
    public interface IAccountService
    {
        Task<string> Register(RegisterDto registerDto, CancellationToken cancellationToken);

        Task<string> Login(LoginDto loginDto, CancellationToken cancellationToken);

        Task<List<UserAccount>> GetUsers(CancellationToken cancellationToken);
    }
}
