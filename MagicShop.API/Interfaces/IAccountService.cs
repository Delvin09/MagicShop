using MagicShop.API.Dtos;
using System.Threading;

namespace MagicShop.API.Interfaces
{
    public interface IAccountService
    {
        Task<string> Register(RegisterDto registerDto, CancellationToken cancellationToken);

        Task<string> Login(LoginDto loginDto, CancellationToken cancellationToken);
    }
}
