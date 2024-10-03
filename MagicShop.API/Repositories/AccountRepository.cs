using MagicShop.API.Interfaces;
using MagicShop.Entity;
using MagicShop.Entity.Models;
using Microsoft.EntityFrameworkCore;

namespace MagicShop.API.Repositories
{
    internal class AccountRepository : IAccountRepository
    {
        private readonly MagicShopContext shopContext;
        private readonly ILogger<AccountRepository> logger;

        public AccountRepository(MagicShopContext shopContext, ILogger<AccountRepository> logger)
        {
            this.shopContext = shopContext;
            this.logger = logger;
        }

        public async Task<UserAccount> AddAccount(UserAccount account, CancellationToken cancellationToken = default)
        {
            await shopContext.AddAsync(account, cancellationToken);
            await shopContext.SaveChangesAsync(cancellationToken);
            return account;
        }

        public async Task<UserAccount?> GetAccount(string login, CancellationToken cancellationToken = default)
        {
            return await shopContext.UserAccounts
                .AsNoTracking()
                .SingleOrDefaultAsync(acc => acc.Login == login);
        }

        public Task<List<UserAccount>> GetAccounts(CancellationToken cancellationToken)
        {
            return shopContext.UserAccounts.AsNoTracking().ToListAsync(cancellationToken);
        }
    }
}
