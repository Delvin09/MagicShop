using MagicShop.Entity.Models;

namespace MagicShop.API.Interfaces
{
    public interface IAccountRepository
    {
        Task<UserAccount> AddAccount(UserAccount account, CancellationToken cancellationToken = default);

        Task<UserAccount?> GetAccount(string login, CancellationToken cancellationToken = default);

        Task<List<UserAccount>> GetAccounts(CancellationToken cancellationToken);
    }
}
